namespace Final_Project_Farrukh_Rakhmanov.Models
{
    public class MemberBook
    {
        public int Id { get; set; }
        public Member? Member { get; set; }

        public int BookId { get; set; }
        public Book? Book { get; set; }
    }
}
