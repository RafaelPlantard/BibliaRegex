using Microsoft.Practices.Prism.Mvvm;

namespace BibliaRegex.Models
{
    /// <summary>
    /// A verse of the bible.
    /// </summary>
    public class Verse : BindableBase
    {
        #region Fields

        private int _number;
        private string _text;

        #endregion

        #region Properties

        /// <summary>
        /// The number or id of the verse.
        /// </summary>
        public int Number
        {
            get { return _number; }
            set { SetProperty(ref _number, value); }
        }

        /// <summary>
        /// The text of the verse.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        #endregion
    }
}