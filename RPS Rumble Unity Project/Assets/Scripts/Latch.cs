public class Latch
{
    public bool isSet { get; private set; } = false;

    public void set(bool value = true)
    {
        isSet = value;
    }

    public void reset()
    {
        isSet = false;
    }
}
