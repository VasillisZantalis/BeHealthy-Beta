namespace BeHealthy.Client.Pages.Patients;

public partial class Index
{
    private string _createUserHref { get; set; } = default!;

    protected override void OnInitialized()
    {
        _createUserHref = "Account/Register";
    }
}
