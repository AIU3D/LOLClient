public delegate void WarningResult();
public class WarningModel
{
    public WarningResult result;
    public string value;

    public WarningModel(string value, WarningResult relust = null)
    {
        this.value = value;
        this.result = relust;
    }

}