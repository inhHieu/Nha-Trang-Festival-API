namespace Festival.Models
{
    public class UserDetail
    {
        public int User_ID { get; set; }

        public int RoleId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int total_subscriptions { get; set; }
        public string top_category_1 { get; set; }
        public string top_category_2 { get; set; }
        public string top_category_3 { get; set; }

        public bool Sex { get; set; }

        public DateTime Age { get; set; }

        public string Avatar { get; set; } = null;
    }
}
