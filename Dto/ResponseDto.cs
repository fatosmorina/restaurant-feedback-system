namespace RestaurantFeedbackSystem.Dto;

public class ResponseDto
{
    public string ResponseText { get; set; }
    
    public ResponseDto(string responseText)
    {
        ResponseText = responseText;
    }
}