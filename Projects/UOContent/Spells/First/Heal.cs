using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.First
{
    public class HealSpell : MagerySpell, ISpellTargetingMobile
    {
        private static readonly SpellInfo _info = new(
            "Heal",
            "In Mani",
            224,
            9061,
            Reagent.Garlic,
            Reagent.Ginseng,
            Reagent.SpidersSilk
        );

        public HealSpell(Mobile caster, Item scroll = null) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.First;

        public void Target(Mobile m)
        {
            if (m.IsDeadBondedPet)
            {
                Caster.SendLocalizedMessage(1060177); // You cannot heal a creature that is already dead!
            }
            else if (m is BaseCreature creature && creature.IsAnimatedDead)
            {
                Caster.SendLocalizedMessage(1061654); // You cannot heal that which is not alive.
            }
            else if (m is Golem)
            {
                Caster.LocalOverheadMessage(MessageType.Regular, 0x3B2, 500951); // You cannot heal that.
            }
            else if (m.Poisoned || MortalStrike.IsWounded(m))
            {
                Caster.LocalOverheadMessage(MessageType.Regular, 0x22, Caster == m ? 1005000 : 1010398);
            }
            else if (CheckBSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                int toHeal;

                if (Core.AOS)
                {
                    toHeal = Caster.Skills.Magery.Fixed / 120;
                    toHeal += Utility.RandomMinMax(1, 4);

                    if (Core.SE && Caster != m)
                    {
                        toHeal = (int)(toHeal * 1.5);
                    }
                }
                else
                {
                    toHeal = (int)(Caster.Skills.Magery.Value * 0.1);
                    toHeal += Utility.Random(1, 5);
                }

                // m.Heal( toHeal, Caster );
                SpellHelper.Heal(toHeal, m, Caster);

                m.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
                m.PlaySound(0x1F2);
            }

            FinishSequence();
        }

        public override void OnCast()
        {
            Caster.Target = new SpellTargetMobile(this, TargetFlags.Beneficial, Core.ML ? 10 : 12);
        }
    }
}
