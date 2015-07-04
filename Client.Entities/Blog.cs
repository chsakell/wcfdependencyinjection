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
    public class Blog : Validatable, IExtensibleDataObject
    {
        #region Variables
        int _id;
        string _name;
        string _url;
        string _owner;
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
        public string Name
        {
            get
            {
                return _name;
            }
            set { _name = value; }
        }
        public string URL
        {
            get
            {
                return _url;
            }
            set { _url = value; }
        }
        public string Owner
        {
            get
            {
                return _owner;
            }
            set { _owner = value; }
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
    class BlogValidator : AbstractValidator<Blog>
    {
        public BlogValidator()
        {
            RuleFor(b => b.Name).NotEmpty();
            RuleFor(b => b.Owner).NotEmpty();
            RuleFor(b => b.ID).GreaterThan(0);
        }
    }
}
