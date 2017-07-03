using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NVCodingTestTask.Models
{
    public class User
    {
        [HiddenInput (DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "User name")]
        [Required(ErrorMessage = "This field cannot be empty")]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "Name length must be between 6 and 64 symbols")]
        [RegularExpression(@"^[A-Za-z0-9]{6,64}$", ErrorMessage = "Name must contains only big and little english letters and digits")]
        public string Name { get; set; }

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "This field cannot be empty")]
        [RegularExpression(@"[A-Za-z0-9-._%+]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Incorrect address format")]
        public string Email { get; set; }

        [Display(Name = "Avatar path")]
        public string Avatar { get; set; }

        [Display(Name = "Skype login")]
        [RegularExpression(@"^[A-Za-z0-9-\._+]{0,64}$", ErrorMessage = "Skype login must contain only big and little english letters and digits")]
        [StringLength(64, ErrorMessage = "Skype login length must be between 3 and 64 symbols")]
        public string SkypeLogin { get; set; }

        [Display(Name = "Signature")]
        public string Signature { get; set; }
    }
}