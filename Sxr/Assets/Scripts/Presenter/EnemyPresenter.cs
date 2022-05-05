using Model;
using View.Interfaces;

namespace Presenter
{
    internal class EnemyPresenter
    {
        private IEnemyView _enemyView;
        private Enemy _enemy;

        public EnemyPresenter(IEnemyView enemyView, Enemy enemy)
        {
            _enemyView = enemyView;
            _enemy = enemy;
        }
    }
}