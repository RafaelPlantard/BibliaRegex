using Microsoft.Practices.Prism.Mvvm;
using System.Collections.Generic;

namespace BibliaRegex.Models
{
    /// <summary>
    /// A chapter of the bible.
    /// </summary>
    public class Chapter : BindableBase
    {
        #region Fields

        private int _number;
        private string _description;
        private IList<Verse> _verses;

        #endregion

        #region Properties

        /// <summary>
        /// The id for this chapter.
        /// </summary>
        public int Number
        {
            get { return _number; }
            set { SetProperty(ref _number, value); }
        }

        /// <summary>
        /// A optional description of this chapter.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        /// <summary>
        /// The verses that this chapter contains.
        /// </summary>
        public IList<Verse> Verses
        {
            get { return _verses; }
            set { SetProperty(ref _verses, value); }
        }

        #endregion
    }
}
