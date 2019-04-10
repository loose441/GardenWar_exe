using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLog
{
    public Vector2Int lastCellPos { get; private set; }
    public Vector2Int destination { get; private set; }

    public MoveLog(Vector2Int _lastUnitCell, Vector2Int _destination)
    {
        lastCellPos = _lastUnitCell;
        destination = _destination;
    }

    public MoveLog Clone()
    {
        return new MoveLog(lastCellPos, destination);
    }
}

public class MoveSolution
{
    public Stack<MoveLog> moveLogs = new Stack<MoveLog>();
    public float evaluation;


    public MoveSolution Clone()
    {
        MoveSolution clone = new MoveSolution();
        clone.evaluation = evaluation;

        Stack<MoveLog> tmp = new Stack<MoveLog>();
        foreach(MoveLog log in moveLogs)
        {
            tmp.Push(log.Clone());
        }

        while (tmp.Count > 0)
        {
            clone.moveLogs.Push(tmp.Pop());
        }

        return clone;
    }
}

public class DamageMap
{
    public DamageCell[,] damageCells = new DamageCell[Board.boardWidth, Board.boardHeight];

    public DamageMap()
    {
        //左下から右上にかけてセルを配置
        for (int x = 0; x < Board.boardWidth; x++)
        {
            for (int y = 0; y < Board.boardHeight; y++)
            {

                damageCells[x, y] = new DamageCell(new Vector2Int(x, y));

            }
        }
    }
}

public class DamageCell
{
    public int damageAmount { get; private set; } = 0;
    public List<UnitBase> attackUnit { get; private set; } = new List<UnitBase>();
    public Vector2Int cellPos { get; private set; }
    
    public DamageCell(Vector2Int pos)
    {
        cellPos = pos;
    }

    public void AddAttackableUnit(int damage,UnitBase unit)
    {
        damageAmount += damage;
        attackUnit.Add(unit);
    }
}

public class EnemyAI : MonoBehaviour
{
    const int solutionNum = 3;
    private MoveSolution[] optimalSolutions = new MoveSolution[solutionNum];

    static int enemyColor;
    const float maxPoint_density = 35;
    const float maxPoint_Line = 45;
    const float maxPoint_Distance = 55;
    const float maxPoint_formation = 0;
    const float levelUpbonus = 200;
    const float attackableBonus = 10;
    const float mageFormationBonus = 2;
    

    private void Start()
    {
        PhaseManager.phaseTransCallBack += PhaseTransed;
    }

    public static void InitializeColor(int color)
    {
        enemyColor = color;
    }

    public void PhaseTransed()
    {
        if (PhaseManager.IsYourTurn())
            return;

        Debug.Log("transed");
        if (PhaseManager.IsMainPhase())
        {
            Debug.Log("Main");
            StartCoroutine(MainPhase());
        }
        else if (PhaseManager.IsBattlePhase())
        {
            Debug.Log("Battle");
            StartCoroutine(BattlePhase());
        }
    }

    private float BoardEvaluation(UnitBase[] playerUnits, UnitBase[] enemyUnits, Board board, bool a = false)
    {
        float evaluation = 0;

        //密集度評価
        evaluation += EvaluateDensity(enemyUnits);

        //陣形評価
        //evaluation += EvaluateFormation(playerUnits, enemyUnits);

        //レベルアップ確認
        foreach(UnitBase unit in enemyUnits)
        {
            if (unit.unitState.CanLevelUp())
                evaluation += levelUpbonus;
        }

        //前線評価
        evaluation += EvaluateLine(enemyUnits);

        //敵との距離評価
        evaluation += EvaluateDistance(playerUnits, enemyUnits);

        //攻撃可能かで評価
        evaluation += IsAttackable(enemyUnits, board);

        //Mageの攻撃範囲に味方が多いか
        evaluation += EvaluateMageFormation(enemyUnits, board);

        if (a)
        {
            Debug.Log("前線評価" + EvaluateLine(enemyUnits) / evaluation);
            Debug.Log("敵との距離評価" + EvaluateDistance(playerUnits, enemyUnits) / evaluation);
            Debug.Log("攻撃可能かで評価" + IsAttackable(enemyUnits, board) / evaluation);
            Debug.Log("Mageの攻撃範囲に味方が多いか" + EvaluateMageFormation(enemyUnits, board) / evaluation);
        }

        return evaluation;
    }

    private float BoardEvaluation(UnitBase[] conbinedUnits, Board board)
    {
        //ユニットをプレイヤーのものと敵のものに分別
        List<UnitBase> enemyUnits = new List<UnitBase>();
        List<UnitBase> playerUnits = new List<UnitBase>();

        foreach (UnitBase unit in conbinedUnits)
        {
            if (unit.unitState.unitColor == enemyColor)
                enemyUnits.Add(unit);
            else
                playerUnits.Add(unit);
        }

        return BoardEvaluation(playerUnits.ToArray(), enemyUnits.ToArray(), board);
    }

    private Vector2 GetCenterPos(UnitBase[] enemyUnits)
    {
        Vector2 centerPos = Vector2.zero;
        foreach (UnitBase unit in enemyUnits)
        {
            centerPos += unit.unitState.currentCell.cellPosition;
        }

        centerPos.x /= enemyUnits.Length;
        centerPos.y /= enemyUnits.Length;

        return centerPos;
    }
    
    private float IsAttackable(UnitBase[] enemyUnits, Board board)
    {
        float totalBonus = 0;

        foreach(UnitBase unit in enemyUnits)
        {
            //各ユニットの攻撃可能範囲を全て取得
            List<BoardCell> attackableArea;
            unit.unitAttack.GetAllAttackableArea(board, out attackableArea);

            foreach(BoardCell attackableCell in attackableArea)
            {
                if (attackableCell.unitBase == null)
                    continue;
                if (attackableCell.unitBase.unitState.unitColor == enemyColor)
                    continue;

                totalBonus += attackableBonus;
                break;
            }
        }

        return totalBonus;
    }

    private float EvaluateMageFormation(UnitBase[] enemyUnits,Board board)
    {
        UnitBase mage = null;
        float bonusAmount = 0;
        foreach (UnitBase unit in enemyUnits)
        {
            if (unit.unitName != "Mage")
                continue;

            mage = unit;
        }

        if (mage == null)
            return 0;

        List<BoardCell> attackableArea;
        mage.unitAttack.GetAllAttackableArea(board, out attackableArea);

        foreach(BoardCell cell in attackableArea)
        {
            if (!cell.ExistUnit())
                continue;

            if (cell.unitBase.unitState.unitColor == enemyColor)
                bonusAmount += mageFormationBonus;
        }

        return bonusAmount;
    }

    private float EvaluateDensity(UnitBase[] enemyUnits)
    {
        Vector2 centerPos = GetCenterPos(enemyUnits);

        //各ユニットと中心からの距離で密集度を計算
        float totalLength = 0;
        foreach(UnitBase unit in enemyUnits)
        {
            totalLength += (unit.unitState.currentCell.cellPosition - centerPos).magnitude;
        }

        //点数に変換
        return maxPoint_density * Mathf.Exp(-totalLength / enemyUnits.Length);
    }

    private float EvaluateLine(UnitBase[] enemyUnits)
    {
        //レベルアップする位置までの距離の和
        float totalDistance = 0;

        //レベルが上がっていない騎士ユニットを前に出す
        foreach(UnitBase unit in enemyUnits)
        {
            if (!unit.canLevelUp || unit.unitState.IsLevelMax())
                continue;

            totalDistance += Mathf.Abs(unit.unitState.currentCell.cellPosition.y
                - UnitState.levelUpLine[enemyColor]);

        }
        
        return maxPoint_Line * Mathf.Exp(-totalDistance * 0.1f);
    }

    private float EvaluateDistance(UnitBase[] playerUnits, UnitBase[] enemyUnits)
    {
        //プレイヤーとの距離をつめる
        //距離*HPで標的を決める
        Vector2 centerPos = GetCenterPos(enemyUnits);

        float minScore = playerUnits[0].unitState.currentHP
            * Vector2.Distance(playerUnits[0].unitState.currentCell.cellPosition, centerPos);

        for(int i = 1; i < playerUnits.Length; i++)
        {
            float score= playerUnits[i].unitState.currentHP
                * Vector2.Distance(playerUnits[i].unitState.currentCell.cellPosition, centerPos);

            if (minScore > score)
                minScore = score;
        }

        //シグモイド関数で正規化
        minScore =  1 / (1 + Mathf.Exp(-minScore));
        return maxPoint_Distance * Mathf.Exp(-minScore);
    }

    private float EvaluateFormation(UnitBase[] playerUnits, UnitBase[] enemyUnits)
    {
        Vector2 enemyToPlayer = GetCenterPos(playerUnits) - GetCenterPos(enemyUnits);
        float cosAngle = Vector2.Dot(Vector2.right, enemyToPlayer.normalized);
        float sinAngle = Mathf.Sqrt(1 - cosAngle * cosAngle);

        //HPの最も少ないユニットを後ろへ配置
        UnitBase weakestUnit = enemyUnits[0];
        foreach(UnitBase unit in enemyUnits)
        {
            if (weakestUnit.unitState.currentHP > unit.unitState.currentHP)
                weakestUnit = unit;
        }

        //enemyToPlayer軸への座標変換
        float weakestUnit_transed = weakestUnit.unitState.currentCell.cellPosition.x * cosAngle
                + weakestUnit.unitState.currentCell.cellPosition.y * sinAngle;

        float amount = 0;
        foreach (UnitBase unit in enemyUnits)
        {
            float unitPos_transed = unit.unitState.currentCell.cellPosition.x * cosAngle;
            unitPos_transed += unit.unitState.currentCell.cellPosition.y * sinAngle;
            amount += unitPos_transed - weakestUnit_transed;

        }

        //補正
        amount *= amount * amount;
        //シグモイド関数で正規化
        return maxPoint_formation / (1 + Mathf.Exp(-amount / enemyUnits.Length));
    }

    private IEnumerator MainPhase()
    {
        List<UnitBase> units;
        Board currentBoard = GameManager.board.Clone(out units);

        //最適解を初期化
        optimalSolutions = new MoveSolution[solutionNum];
        
        //思考開始
        yield return StartCoroutine(MainPhaseThinking(currentBoard, units.ToArray(), new MoveSolution()));
        //思考終了


        //移動できない場合
        if (optimalSolutions[0] == null)
        {
            PhaseManager.PhaseTrans();
            yield break;
        }

        //最適解候補からランダムに１つ抽出
        int picked = Random.Range(0, solutionNum);
        

        //スタックの順番を逆転
        Stack<MoveLog> convertedLog = new Stack<MoveLog>();
        while (optimalSolutions[picked].moveLogs.Count > 0)
        {
            convertedLog.Push(optimalSolutions[picked].moveLogs.Pop());
        }
        
        while (convertedLog.Count > 0)
        {
            MoveLog log = convertedLog.Pop();
            UnitBase unit = GameManager.board.FindCell(log.lastCellPos).unitBase;
            BoardCell destination = GameManager.board.FindCell(log.destination);
            
            yield return StartCoroutine(UnitController.MoveEvent(unit, destination));
        }

        UnitBase[] playerUnits = GameManager.GetUnitTeam(PlayerController.playerColor).unitList.ToArray();
        UnitBase[] enemyUnits = GameManager.GetUnitTeam(enemyColor).unitList.ToArray();
        Debug.Log(BoardEvaluation(playerUnits, enemyUnits, GameManager.board, true));
    }
    

    private IEnumerator MainPhaseThinking(Board currentBoard, UnitBase[] units, MoveSolution currentSolution)
    {
        //ユニットの移動が終了した場合
        if (GameManager.IsMainPhaseEnd(units))
        {
            //盤面の評価
            currentSolution.evaluation = BoardEvaluation(units, currentBoard);
            AddSolution(currentSolution, ref optimalSolutions);
            yield break;
        }

        //盤面シミュレーション
        foreach (UnitBase unit in units)
        {
            if (unit.unitState.unitColor != enemyColor)
                continue;
            if (unit.unitState.ActEnd())
                continue;



            List<BoardCell> movableArea;
            unit.unitMovement.GetSelectableCell(currentBoard, out movableArea);

            if (movableArea.Count == 0)
                continue;
            

            //全ての行動の内、評価の低い半分を間引く
            MoveSolution[] potentSolution = new MoveSolution[(movableArea.Count + 1) / 2];
            foreach (BoardCell nextCell in movableArea)
            {
                MoveSolution newSolution = currentSolution.Clone();

                //ユニットを移動
                newSolution.moveLogs.Push(new MoveLog(unit.unitState.currentCell.cellPosition, nextCell.cellPosition));
                UnitController.MoveUnit(unit, nextCell);

                //現時点での評価値を計算、有力かどうかを検証
                newSolution.evaluation = BoardEvaluation(units, currentBoard);
                AddSolution(newSolution, ref potentSolution);
                

                //動かしたユニットを元へ戻す
                UnitController.UndoUnitMove(unit, currentBoard.FindCell(newSolution.moveLogs.Peek().lastCellPos));
            }
            
            
            //残った行動を検証
            foreach (MoveSolution solution in potentSolution)
            {
                //移動
                UnitController.MoveUnit(unit, currentBoard.FindCell(solution.moveLogs.Peek().destination));

                yield return StartCoroutine(MainPhaseThinking(currentBoard, units, solution));

                //動かしたユニットを元へ戻す
                MoveLog log = solution.moveLogs.Pop();
                UnitController.UndoUnitMove(unit, currentBoard.FindCell(log.lastCellPos));
            }
            
        }
    }

    private IEnumerator BattlePhase()
    {
        UnitBase[] playerUnits = GameManager.GetUnitTeam(PlayerController.playerColor).unitList.ToArray();
        UnitBase[] enemyUnits = GameManager.GetUnitTeam(enemyColor).unitList.ToArray();

        DamageMap damageMap = CreateDamageMap(enemyUnits);
        //倒せる敵がいるか検索、倒せるなら倒す
        foreach(DamageCell cell in damageMap.damageCells)
        {
            UnitBase unit = GameManager.board.boardCells[cell.cellPos.x, cell.cellPos.y].unitBase;

            if (unit == null)
                continue;
            if (unit.unitState.unitColor == enemyColor)
                continue;

            if (cell.damageAmount > unit.unitState.currentHP)
                yield return StartCoroutine(OptimumAttack(cell, unit));

            //ターンが終了してないか確認(全てのユニットの攻撃が終わった場合自動的にターンが変わるため)
            if (!PhaseManager.IsBattlePhase())
                yield break;
        }



        //最も与ダメージが大きい攻撃の仕方を行う
        foreach (UnitBase enemyUnit in enemyUnits)
        {
            if (enemyUnit.unitState.ActEnd())
                continue;


            BoardCell mostDamageableCell = FindMostDamageableCell(enemyUnit);

            if (mostDamageableCell == null)
                continue;

            yield return StartCoroutine(UnitController.AttackEvent(enemyUnit, mostDamageableCell));


            //ターンが終了してないか確認
            if (!PhaseManager.IsBattlePhase())
                yield break;
        }


        //Mageが攻撃してない場合、味方を回復させるか判断
        foreach (UnitBase enemyUnit in enemyUnits)
        {
            if (enemyUnit.unitName != "Mage")
                continue;

            if (enemyUnit.unitState.ActEnd())
                continue;


            List<BoardCell> attackableArea;
            enemyUnit.unitAttack.GetAllAttackableArea(GameManager.board, out attackableArea);
            foreach(BoardCell cell in attackableArea)
            {
                if (!cell.ExistUnit())
                    continue;

                if (cell.unitBase.unitState.unitColor != enemyColor)
                    continue;


                //HPの減ったユニットを見つけ次第、回復
                if (cell.unitBase.unitState.currentHP != cell.unitBase.maxHp)
                {
                    yield return StartCoroutine(UnitController.AttackEvent(enemyUnit, cell));


                    //ターンが終了してないか確認
                    if (!PhaseManager.IsBattlePhase())
                        yield break;

                    break;
                }

            }

        }


        //ターン終了
        yield return new WaitForSeconds(0.5f);
        PhaseManager.PhaseTrans();
    }

    private IEnumerator OptimumAttack(DamageCell damageCell, UnitBase forcusedUnit)
    {
        //ソート
        SortDamageCell(damageCell);
        
        //最小攻撃回数を探す
        int minAttackUnitNum = 0;
        int atkAmount = 0;
        for(int i = damageCell.attackUnit.Count - 1; i >= 0; i--)
        {
            minAttackUnitNum++;
            atkAmount += damageCell.attackUnit[i].unitAttack.atk;

            if (atkAmount > forcusedUnit.unitState.currentHP)
                break;
        }

        
        foreach(UnitBase unit in damageCell.attackUnit)
        {
            //攻撃方向を探す
            List<BoardCell> selectableArea;
            List<BoardCell> attackableArea;
            unit.unitAttack.GetSelectableCell(GameManager.board, out selectableArea);

            foreach(BoardCell selectableCell in selectableArea)
            {
                unit.unitAttack.GetAttackableArea(GameManager.board, selectableCell.cellPosition - unit.unitState.currentCell.cellPosition, out attackableArea);
                if (!attackableArea.Contains(forcusedUnit.unitState.currentCell))
                    continue;

                yield return StartCoroutine(
                    UnitController.AttackEvent(unit,selectableCell));
                break;
            }
            
            
            if (forcusedUnit.unitState.currentHP <= 0)
            {
                yield break;
            }
        }



    }
    

    private DamageMap CreateDamageMap(UnitBase[] units)
    {
        DamageMap damageMap = new DamageMap();

        foreach(UnitBase unit in units)
        {
            if (unit.unitState.ActEnd())
                continue;

            List<BoardCell> attackableArea;
            unit.unitAttack.GetAllAttackableArea(GameManager.board, out attackableArea);

            foreach(BoardCell attackableCell in attackableArea)
            {
                damageMap.damageCells[attackableCell.cellPosition.x, attackableCell.cellPosition.y]
                    .AddAttackableUnit(unit.unitAttack.atk, unit);
            }
        }

        return damageMap;
    }

    private BoardCell FindMostDamageableCell(UnitBase unit)
    {
        List<BoardCell> selectableArea;
        unit.unitAttack.GetSelectableCell(GameManager.board, out selectableArea);

        
        int attackedUnitNum = 0;
        BoardCell mostDamageableCell = null;
        List<BoardCell> attackableArea;

        foreach (BoardCell selectableCell in selectableArea)
        {
            unit.unitAttack.GetAttackableArea(GameManager.board
                , selectableCell.cellPosition - unit.unitState.currentCell.cellPosition
                , out attackableArea);

            //攻撃可能なユニット数を計算
            int num = 0;
            foreach (BoardCell attackableCell in attackableArea)
            {

                if (!attackableCell.ExistUnit())
                    continue;

                if (attackableCell.unitBase.unitState.unitColor == enemyColor)
                    continue;

                num++;
            }


            if (attackedUnitNum < num)
            {
                attackedUnitNum = num;
                mostDamageableCell = selectableCell;
                continue;
            }
            else if (attackedUnitNum == num && attackedUnitNum != 0)
            {
                //攻撃対象数が同じ場合は近くのユニットを攻撃
                if(Board.DistanceBetweenCells(mostDamageableCell,unit.unitState.currentCell)
                    >= Board.DistanceBetweenCells(selectableCell, unit.unitState.currentCell))
                {
                    mostDamageableCell = selectableCell;
                    continue;
                }

            }
        }

        return mostDamageableCell;
    } 

    private void SortDamageCell(DamageCell damageCell)
    {
        //攻撃力を昇順にソート
        if (damageCell.attackUnit.Count < 2)
            return;


        for(int i = damageCell.attackUnit.Count - 1; i > 0; i--)
        {
            for(int k = i - 1; k >= 0; k--)
            {


                if (damageCell.attackUnit[i].unitAttack.atk > damageCell.attackUnit[k].unitAttack.atk)
                    continue;

                
                if(damageCell.attackUnit[i].unitAttack.atk < damageCell.attackUnit[k].unitAttack.atk)
                {
                    UnitBase tmp = damageCell.attackUnit[k];
                    damageCell.attackUnit.RemoveAt(k);
                    damageCell.attackUnit.Add(tmp);
                    continue;
                }


            }
        }
    }

    private void SortSolutionUpward(ref MoveSolution[] solutions)
    {
        if (solutions.Length < 2)
            return;
        

        for(int i = 0; i < solutions.Length - 1; i++)
        {
            for (int k = i + 1; k < solutions.Length; k++)
            {
                if (solutions[i].evaluation > solutions[k].evaluation)
                {
                    MoveSolution tmp = solutions[i];
                    solutions[i] = solutions[k];
                    solutions[k] = tmp;
                }
            }
        }
        
    }

    

    private void AddSolution(MoveSolution candidate,ref MoveSolution[] potentSolution)
    {
        if (potentSolution[0] == null)
        {
            for (int i = 0; i < potentSolution.Length; i++)
            {
                potentSolution[i] = candidate.Clone();
            }
            return;
        }

        for (int i = 0; i < potentSolution.Length; i++)
        {
            if (potentSolution[i].evaluation < candidate.evaluation)
            {
                potentSolution[i] = candidate.Clone();
                SortSolutionUpward(ref potentSolution);
                return;
            }
        }
    }
}