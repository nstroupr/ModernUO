namespace Server.Items
{
  public class PoisonScroll : SpellScroll
  {
    [Constructible]
    public PoisonScroll(int amount = 1) : base(19, 0x1F40, amount)
    {
    }

    public PoisonScroll(Serial serial) : base(serial)
    {
    }

    public override void Serialize(GenericWriter writer)
    {
      base.Serialize(writer);

      writer.Write(0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
      base.Deserialize(reader);

      int version = reader.ReadInt();
    }
  }
}