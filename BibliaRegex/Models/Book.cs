using Microsoft.Practices.Prism.Mvvm;
using System.Collections.Generic;

namespace BibliaRegex.Models
{
    /// <summary>
    /// A book of the bible.
    /// </summary>
    public class Book : BindableBase
    {
        #region Fields

        private string _initials;
        private string _name;
        private List<Chapter> _chapters;

        #endregion

        #region Properties

        /// <summary>
        /// The initial for this book
        /// </summary>
        public string Initials
        {
            get { return _initials; }
            set { SetProperty(ref _initials, value); }
        }

        /// <summary>
        /// The name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        /// <summary>
        /// The chapters of the book.
        /// </summary>
        public List<Chapter> Chapters
        {
            get { return _chapters; }
            set { SetProperty(ref _chapters, value); }
        }

        #endregion
    }
}
