namespace Final_Project_Farrukh_Rakhmanov.Models
{
    public class SearchViewModel
    {
        //Book model fields
        public List<Book> Books { get; set; }
        public List<string> Genres { get; set; }
        public string SelectedGenre { get; set; }

        //Member model fields
        public List<Member> Members { get; set; }

        //Common fields
        public string SearchTerm { get; set; }
        public string SortBy { get; set; }
        public string FilterBy { get; set; }
    }
}
