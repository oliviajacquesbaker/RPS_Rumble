public struct Latch
{
    public bool isSet { get; private set; }

    public void set(bool value = true)
    {
        isSet = value;
    }

    public void reset()
    {
        isSet = false;
    }
}
