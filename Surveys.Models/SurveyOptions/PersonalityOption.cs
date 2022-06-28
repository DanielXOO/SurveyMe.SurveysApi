namespace Surveys.Models.SurveyOptions;

public sealed class PersonalityOption
{
    public Guid PersonalityOptionId { get; set; }
    
    public Guid SurveyOptionsId { get; set; }
    
    public string PropertyName { get; set; }

    public bool IsRequired { get; set; }

    public OptionType Type { get; set; }
}