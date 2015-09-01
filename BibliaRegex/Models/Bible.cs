using Microsoft.Practices.Prism.Mvvm;
using System.Collections.Generic;

namespace BibliaRegex.Models
{
    /// <summary>
    /// A bible.
    /// </summary>
    public class Bible : BindableBase
    {
        #region Fields

        private string _name;
        private string _initials;
        private string _language;
        private List<Book> _oldTestament;
        private List<Book> _newTestament;

        #endregion

        #region Properties

        /// <summary>
        /// The name of the bible.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        /// <summary>
        /// Initials for this version of the bible.
        /// </summary>
        public string Initials
        {
            get { return _initials; }
            set { _initials = value; }
        }

        /// <summary>
        /// The name of the bible.
        /// </summary>
        public string Language
        {
            get { return _language; }
            set { SetProperty(ref _language, value); }
        }

        /// <summary>
        /// The books from the old testament
        /// </summary>
        public List<Book> OldTestament
        {
            get { return _oldTestament; }
            set { SetProperty(ref _oldTestament, value); }
        }

        /// <summary>
        /// The books from the new testament
        /// </summary>
        public List<Book> NewTestament
        {
            get { return _newTestament; }
            set { SetProperty(ref _newTestament, value); }
        }

        #endregion
    }
}
