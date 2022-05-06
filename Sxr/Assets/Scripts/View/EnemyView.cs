using Model;
using Model.GameField;
using Presenter;
using UnityEngine;
using View.Interfaces;

namespace View
{
    internal class EnemyView : MonoBehaviour, IEnemyView
    {
        private EnemyPresenter _enemyPresenter;

        public void Init(EnemyPresenter enemyPresenter)
        {
            _enemyPresenter = enemyPresenter;
        }
    }
}