namespace MovieAPI.Dtos
{
    public class UpdateActorDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Biography { get; set; }
        public List<ActorAwardsDto>? Awards { get; set; }

    }
}
