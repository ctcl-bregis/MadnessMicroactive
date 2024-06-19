using System.Numerics;
using Walgelijk;

namespace MadnessMicroactive;

public class Firearm : IWeapon
{
    public IReadableTexture Texture = Walgelijk.Texture.ErrorTexture;
    public Vector2[] HoldPoints = Array.Empty<Vector2>();
    public Sound[] Sounds = Array.Empty<Sound>();

    public Vector2 BarrelOut;
    public Vector2 EjectorPort = new Vector2(0, 0);
    public float Dispersion;
    public float Damage;
    public float Recoil;
    public float PierceCoeff = 0;
    public float AutoInterval;
    public int BulletsPerShot = 1;

    public int MaxRounds = 20;

    public string DisplayedName = "";

    public string Name { get; }

    public Vector2 EjectorPos;

    public Firearm(string name)
    {
        Name = name;
    }

    public Vector2 GetWorldBarrelPos(TransformComponent transform, bool flipped)
    {
        var b = BarrelOut;
        if (flipped)
            b.X *= -1;
        return Vector2.Transform(b / transform.Scale, transform.LocalToWorldMatrix);
    }

    public void Use(Scene scene, WeaponComponent weapon, CharacterComponent wielder, float dt)
    {
        if (weapon.RemainingRounds <= 0)
            return;

        if (AutoInterval > 0)
            weapon.AutoTimer += dt;

        var transform = scene.GetComponentFrom<TransformComponent>(weapon.Entity);
        var outPos = GetWorldBarrelPos(transform, wielder.Flipped);

        if (weapon.AutoTimer > Math.Max(float.Epsilon, AutoInterval))
        {
            weapon.AutoTimer = 0;

            wielder.Recoil += Recoil;

            var dir = Utilities.AngleToVector(transform.Rotation) * (wielder.Flipped ? -1 : 1);

            scene.Game.AudioRenderer.PlayOnce(Utilities.PickRandom(Sounds));

            for (int i = 0; i < BulletsPerShot; i++)
                scene.AttachComponent(scene.CreateEntity(), new BulletComponent
                {
                    Position = outPos,
                    LastPosition = outPos,
                    Velocity = Utilities.AngleToVector(transform.Rotation + Utilities.RandomFloat(-Dispersion, Dispersion)) * (wielder.Flipped ? -400 : 400),
                    PierceCoeff = PierceCoeff,
                    Damage = Damage
                });

            Prefabs.CreateMuzzleflash(scene, outPos, dir, Damage / 20);

            // Probably a better way to do this
            if (wielder.Flipped) {
                EjectorPos = transform.Position - EjectorPort;
            } else {
                EjectorPos = transform.Position + EjectorPort;
            }

            MandessUtils.EjectCasing(scene, transform, EjectorPos, Utilities.AngleToVector(transform.Rotation + 90) * Utilities.RandomFloat(500, 900));

            weapon.RemainingRounds--;
            if (weapon.RemainingRounds == 0)
            {
                scene.Game.AudioRenderer.PlayOnce(SoundCache.Instance.LoadSoundEffect(Resources.Load<FixedAudioData>("wep/out_of_ammo.wav")));
                return;
            }
        }
    }
}
