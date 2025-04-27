namespace MovieAPI.models
{
    public class ActorAward
    {
        public int ActorId { get; set; }
        public int AwardId { get; set; }
        public int YearOfHonor { get; set; }
        public Actor Actor { get; set; }
        public Award Award { get; set; }

    }
}
