public interface IDamageable : IObject
{
	int Health { get; }
	bool IsDestroyed { get; }
	void ApplyDamage(int damage);
}