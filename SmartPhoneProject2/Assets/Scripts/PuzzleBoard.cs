using UnityEngine;

public class PuzzleBoard : MonoBehaviour
{
    [Header("Prefabと生成先")]
    // 生成する石のPrefab
    [SerializeField]
    private PuzzleBlock piecePrefab;
    [SerializeField]
    // 石を配置する親オブジェクト
    private RectTransform pieceRoot;

    [Header("属性画像")]
    // 石のSprite(火、水、木、光、闇の順番)
    [SerializeField]
    private Sprite[] attributeSprites = new Sprite[5];
    [Header("石の配置設定")]
    // 石1個の大きさ
    [SerializeField]
    private float cellSize = 100f;
    // 石同士の間隔
    [SerializeField]
    private float spaceing = 0f;
    // 左上の石の中心座標を保持する
    private Vector2 firstCenter;
    // 盤面を管理する2次元配列
    private PuzzleBlock[,] board;
    // 石同士の間隔を参照するためのプロパティ
    public float Spaceing => spaceing;
    [Header("盤面サイズ")]
    // 横方向のマス数
    [SerializeField, Range(1,6)]
    private int width = 6;
    // 縦方向のマス数
    [SerializeField, Range(1, 6)]
    private int height = 6;

    private void Start()
    {
        
    }

    private void Update()
    {
    }

    /// <summary>
    /// 石の大きさと最初の石の中心位置を設定する
    /// </summary>
    private void ConfigureCells()
    {
        firstCenter = 
            new Vector2(cellSize / 2f, cellSize / 2f);
    }

    /// <summary>
    /// 指定した位置の座標を求める
    /// </summary>
    /// <param name="x">盤面のX座標</param>
    /// <param name="y">盤面のY座標</param>
    /// <returns></returns>
    private Vector2 GetCellPosision(int x, int y)
    {
        // 石の大きさと間隔を合計する
        float step = cellSize + spaceing;
        // UIのY座標は下方向がマイナス
        return new Vector2(
            firstCenter.x + x * step,
            -(firstCenter.y + y * step)
            );
    }

    /// <summary>
    /// パズル盤面を生成する
    /// </summary>
    private void Build()
    {
        // Inspectprの設定を確認する
        if (piecePrefab == null || pieceRoot == null)
        {
            Debug.LogError(
                "Piece Prefabまたは、PieceRootが未設定です。"
                );
            return;
        }

        if (attributeSprites == null
            || attributeSprites.Length != 5)
        {
            Debug.LogError(
                "AttributeSpritesに５種類の画像を設定してください。"
                );
            return;
        }

        for (int i = 0; i < attributeSprites.Length; i++)
        {
            if(attributeSprites[i] != null)
            {
                Debug.LogError(
                    $"AttributeSpriteの{i}番が未設定です。"
                    );
                return;
            }

            // 石の配置情報を準備する
            ConfigureCells();
            // 2次元配列にする
            board = new PuzzleBlock[width, height];

            // 上から順番に行を処理する
            for(int y  = 0; y < height; y++)
            {
                // 左から順番に列を処理する
                for (int x = 0; x < width; x++)
                {
                    // 0～4の属性をランダムに選ぶ
                    PuzzleAttribute type = (PuzzleAttribute)
                        Random.Range(0,attributeSprites.Length);
                    // Prefabから石を生成する
                    PuzzleBlock piece = Instantiate(
                        piecePrefab,
                        pieceRoot,
                        false
                        );
                    // Hierarchyで確認しやすい名前をつける
                    piece.name = $"Piece_{x}_{y}";
                    // 座標、属性、画像を設定する
                    piece.Initializer(
                        type,
                        x,
                        y,
                        attributeSprites[(int)type]
                        );

                    // 石のRectTranceformを取得する
                    RectTransform rect = 
                        piece.GetComponent<RectTransform>();
                    // 左上を基準に配置する
                    rect.anchorMin = new Vector2(0f, 1f);
                    rect.anchorMax = new Vector2(0f, 1f);
                    // 石の中心を基準にする
                    rect.pivot = new Vector2(0.5f, 0.5f);
                    // Transformを初期化する
                    rect.localScale = Vector3.one;
                    rect.localRotation = Quaternion.identity;
                    // 石の大きさを設定する
                    rect.sizeDelta = new Vector2(
                        cellSize,
                        cellSize
                        );
                    // マスの大きさから表示位置を決める
                    rect.anchoredPosition =
                        GetCellPosision(x, y);
                    // 2次元配列に登録する
                    board[x, y] = piece;
                }
                Debug.Log(
                    $"盤面表示完了 : {width}x{height} = {width * height}個"
                    );
                Debug.Log(
                    $"左上 : {board[0, 0].name}" +
                    $"{GetCellPosision(0, 0)}"
                    );
            }
        }
    }
}
