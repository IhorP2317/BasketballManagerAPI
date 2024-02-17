﻿using BasketballManagerAPI.Helpers.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace BasketballManagerAPI.Dto.AwardDto {
    public class AwardUpdateDto {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required!")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Name can only contain letters, numbers, and spaces!")]
        public string Name { get; set; } = null!;


        [Required(ErrorMessage = "Date is required!")]
        [DateTimeFormat("yyyy-MM-dd", ErrorMessage = "Date Format must be in the format yyyy-MM-dd.")]
        public string Date { get; set; } = null!;

    }
}
