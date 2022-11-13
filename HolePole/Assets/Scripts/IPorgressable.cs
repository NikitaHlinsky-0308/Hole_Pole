
public interface IPorgressable
{
    public int CurrentLvl { get; set; }
    const int MinLvl = 1;
    const int MaxLvl = 3;

    public void IncreaseLvl();
    public void DecreaseLvl();
}
