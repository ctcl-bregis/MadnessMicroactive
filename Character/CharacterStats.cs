namespace MadnessMicroactive;

public struct CharacterStats
{
    public float MeleeDamage;
    public float MaxHealth;
    public float RegenSpeed;
    public int AiBusinessTolerance;

    public static CharacterStats Grunt => new CharacterStats
    {
        MaxHealth = 80,
        MeleeDamage = 15,
        RegenSpeed = 30
    };

    public static CharacterStats Agent => new CharacterStats
    {
        MaxHealth = 80,
        MeleeDamage = 15,
        RegenSpeed = 30
    };

    public static CharacterStats Engineer => new CharacterStats
    {
        MaxHealth = 80,
        MeleeDamage = 15,
        RegenSpeed = 40,
        AiBusinessTolerance = 1,
    };

    public static CharacterStats Soldat => new CharacterStats
    {
        MaxHealth = 120,
        MeleeDamage = 15,
        RegenSpeed = 50,
        AiBusinessTolerance = 2,
    };   
    
    public static CharacterStats Boss => new CharacterStats
    {
        MaxHealth = 8000,
        MeleeDamage = 100,
        RegenSpeed = 10,
        AiBusinessTolerance = int.MaxValue,
    };    

    public static CharacterStats Player => new CharacterStats
    {
        MaxHealth = 500,
        MeleeDamage = 15,
        RegenSpeed = 60,
        AiBusinessTolerance = int.MaxValue,
    };
}
