// Fabulous Adventures in Data Structures and Algorithms
// Eric Lippert
// Chapter 6
abstract class GameObject
{
    public virtual string Name => this.GetType().Name;
}
abstract class Weapon : GameObject { }
class Longsword : Weapon { }
class Wand : Weapon { }
class Dagger : Weapon { }

abstract class CharacterClass : GameObject
{
    public abstract bool CanWield(Weapon weapon);
}
class Wizard : CharacterClass
{
    public override bool CanWield(Weapon weapon) => weapon is Dagger or Wand;
}
class Warrior : CharacterClass
{
    public override bool CanWield(Weapon weapon) => weapon is Dagger or Longsword;
}
class Thief : CharacterClass
{
    public override bool CanWield(Weapon weapon) => weapon is Dagger;
}
abstract class Monster : GameObject { }
class Vampire : Monster { }
class Werewolf : Monster { }

