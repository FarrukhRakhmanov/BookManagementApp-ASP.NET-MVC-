using Final_Project_Farrukh_Rakhmanov.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Final_Project_Farrukh_Rakhmanov.Models
{
    public class Member : User
    {
         /// <summary>
        /// Member's contact phone number
        /// Validated using built-in Phone attribute to ensure proper format
        /// </summary>
        [ValidPhone]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Member's email address
        /// Validated using built-in EmailAddress attribute to ensure proper format
        /// </summary>
        [ValidEmail]
        public string? Email { get; set; }

        /// <summary>
        /// Foreign key reference to currently checked out book
        /// Nullable to allow members without checked out books
        /// </summary>

        // Navigation property
        public ICollection<MemberBook>? MemberBooks { get; set; }


        public static List<Member> SearchMembers(List<Member> members, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return members;
            }
            searchTerm = searchTerm.ToLower();

            return members.
                Where(b =>
                   b.Name.ToLower().Contains(searchTerm)
                || b.Email.ToLower().Contains(searchTerm)
                || b.PhoneNumber.ToLower().Contains(searchTerm)).ToList();
        }
        public static List<Member> SortMembers(List<Member> members, string sortBy)
        {
            switch (sortBy)
            {
                case "name":
                    return members.OrderBy(b => b.Name).ToList();
                case "name_desc":
                    return members.OrderByDescending(b => b.Name).ToList();
                case "email":
                    return members.OrderBy(b => b.Email).ToList();
                case "email_desc":
                    return members.OrderByDescending(b => b.Email).ToList();
                case "phone":
                    return members.OrderBy(b => b.PhoneNumber).ToList();
                case "phone_desc":
                    return members.OrderByDescending(b => b.PhoneNumber).ToList();
                default:
                    return members;
            }
        }

        public static List<Member> FilterByHasIssuedBook(List<Member> members, string filterBy)
        {
            switch (filterBy)
            {
                case "hasIssuedBook":
                    return members.Where(b => b.MemberBooks.Count > 0).ToList();
                case "noIssuedBook":
                    return members.Where(b => b.MemberBooks.Count == 0).ToList();
                default:
                    return members.OrderBy(b => b.Name).ToList();
            }
        }
    }
}