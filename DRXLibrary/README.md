# DRX Library
DRX Library is a collection of utilities and models that are shared among all applications that can interface with, open, or modify DRX documents and stores.\
You may use DRX Library or parts of its code in your own application, provided you follow the license terms.

## Examples

### Creating and saving a new document
This example creates and saves new document with the title "foo" and the content "Hello World!" using the PlainText encoding.\
In this example, a BCLStorage IFile is used as the parameter to the ToFile method.

```csharp
var document = new DrxDocument();
var header = document.Header;

header.Title = "foo";
header.BodyType = DrxBodyType.PlainText;

document.Body = Encoding.UTF8.GetBytes("Hello World!");
await document.ToFileAsync(file);
```

### Loading a document
This example loads a document from a file and prints the body out to the console. It assumes the body type is PlainText.\
In this example, a BCLStorage IFile is used as the parameter to the FromFile method.
```csharp
var document = await DrxParser.FromFileAsync(file);
Console.WriteLine(Encoding.UTF8.GetString(document.Body));
```