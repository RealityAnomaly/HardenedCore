using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DRXNextGeneration.ViewModels;

namespace DRXNextGeneration.Common.Groupings
{
    public class DrxDocumentTimeGrouping : IGrouping<DateTime, DrxDocumentViewModel>
    {
        private IList<DrxDocumentViewModel> _documents;
        public DateTime Key { get; }

        public DrxDocumentTimeGrouping(DateTime dateTime, IEnumerable<DrxDocumentViewModel> documents)
        {
            Key = dateTime;
            _documents = new List<DrxDocumentViewModel>(documents);
        }

        public IEnumerator<DrxDocumentViewModel> GetEnumerator() => _documents.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _documents.GetEnumerator();
    }
}
