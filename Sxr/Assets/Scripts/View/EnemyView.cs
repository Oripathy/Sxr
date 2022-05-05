using Model;
using Model.GameField;
using Presenter;
using UnityEngine;
using View.Interfaces;

namespace View
{
    internal class EnemyView : MonoBehaviour, IEnemyView
    {
        private GameFieldModel _gameFieldModel;
        private EnemyPresenter _enemyPresenter;

        public void Init(GameFieldModel gameFieldModel, Enemy enemy)
        {
            _enemyPresenter = new EnemyPresenter(this, enemy);
        }
    }
}