using Core.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entities
{
    public class Article : Validatable, IExtensibleDataObject
    {
        #region Variables
        int _id;
        string _title;
        string _contents;
        string _author;
        string _url;
        int _blogID;
        #endregion

        #region Properties
        public int ID
        {
            get
            {
                return _id;
            }
            set { _id = value; }
        }
        public string Title
        {
            get
            {
                return _title;
            }
            set { _title = value; }
        }
        public string Contents
        {
            get
            {
                return _contents;
            }
            set { _contents = value; }
        }
        public string Author
        {
            get
            {
                return _author;
            }
            set { _author = value; }
        }
        public string URL
        {
            get
            {
                return _url;
            }
            set { _url = value; }
        }
        public int BlogID
        {
            get
            {
                return _blogID;
            }
            set { _blogID = value; }
        }
        #endregion 

        #region IExtensibleDataObject

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion

        #region Validation
        protected override IValidator GetValidator()
        {
            return new BlogValidator();
        }
        #endregion
    }

    // Validator Class
    class ArticleValidator : AbstractValidator<Article>
    {
        public ArticleValidator()
        {
            RuleFor(a => a.Author).NotEmpty();
            RuleFor(a => a.BlogID).GreaterThan(0);
            RuleFor(a => a.Contents).NotEmpty();
        }
    }
}
