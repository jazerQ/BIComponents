namespace Application.Scatter;

public class GetRatingAndCommentsDtoResponse
{
    public int Id { get; set; }
    
    public float Rating { get; set; }

    public int CountOfComments { get; set; }

    public string Name { get; set; }
}