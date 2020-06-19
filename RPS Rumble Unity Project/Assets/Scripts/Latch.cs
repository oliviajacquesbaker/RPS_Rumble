public class Latch
{
    public bool isSet { get; private set; } = false;

    public void set()
    {
        isSet = true;
    }

    public void reset()
    {
        isSet = false;
    }
}
