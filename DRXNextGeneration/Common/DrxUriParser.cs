using System;
using Windows.Foundation;
using static DRXNextGeneration.Common.DrxCommand;

namespace DRXNextGeneration.Common
{
    public static class DrxUriParser
    {
        public static DrxCommand Parse(Uri uri)
        {
            var segments = uri.Segments;

            var action = "open";
            if (!string.IsNullOrWhiteSpace(uri.Query))
            {
                var decoder = new WwwFormUrlDecoder(uri.Query);
                action = decoder.GetFirstValueByName("action");
            }

            // Examples:
            // drx://localhost/stores/0d0a0dc4-4ed5-479a-b6db-706ca6e80cb6/documents/70abd4d3-46fb-4707-bf30-a8c29794835d?action=properties
            // drx://localhost[?action={new/restore}]
            // drx://localhost/[store][?action={delete/properties/backup}]
            // drx://localhost/[store]/documents[?action={new}]
            // drx://localhost/[store]/documents/[document][?action={delete/properties}]
            // drx://localhost/[store]/flags[?action={new}]
            // drx://localhost/[store]/flags/[flag][?action={delete/properties]]
            // drx://192.168.1.27:43105/stores/0d0a0dc4-4ed5-479a-b6db-706ca6e80cb6 (remote)

            if (uri.Host != "localhost")
                return null; // TODO: remote host not yet implemented

            switch (segments[1].Trim('/'))
            {
                case "stores":
                    // We're executing a command on the store service
                    if (segments.Length <= 2)
                        return new DrxCommand { Command = action, Subject = DrxCommandSubject.StoreService };

                    if (!Guid.TryParse(segments[2].Trim('/'), out var storeId)) return null;
                    var command = new DrxCommand
                    {
                        Command = action,
                        Parameters = {["storeId"] = storeId}
                    };

                    // We're executing a command on the store
                    if (segments.Length == 3)
                    {
                        command.Subject = DrxCommandSubject.Store;
                        return command;
                    }

                    var isRoot = segments.Length == 4;
                    switch (segments[3].Trim('/'))
                    {
                        case "documents":
                            command.Subject = isRoot ? DrxCommandSubject.StoreDocuments : DrxCommandSubject.Document;
                            break;
                        case "flags":
                            command.Subject = isRoot ? DrxCommandSubject.StoreFlags : DrxCommandSubject.Flag;
                            break;
                        default:
                            return null;
                    }

                    if (isRoot) return command;
                    if (!Guid.TryParse(segments[4].Trim('/'), out var ctxId)) return null;
                    command.Parameters["contextId"] = ctxId;

                    return command;
                default:
                    return null;
            }
        }
    }
}
