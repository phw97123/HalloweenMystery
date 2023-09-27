using Entites;

public interface IEnemyState
{
    void Enter(MeleeEnemyController enemy, EntityController entityController);
    void Update(MeleeEnemyController enemy, EntityController entityController);
    void Exit(MeleeEnemyController enemy, EntityController entityController);
}
