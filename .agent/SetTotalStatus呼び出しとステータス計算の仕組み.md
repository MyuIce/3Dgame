# SetTotalStatus呼び出しとステータス計算の仕組み

## 概要
このドキュメントは、プレイヤーのステータス計算システムにおける`SetTotalStatus`メソッドの呼び出しから、最終的なステータス計算までの全体的な流れをまとめたものです。

---

## 1. システム全体の流れ

```
[装備変更] → [CalculateTotalStatus] → [ステータス計算] → [SetTotalStatus] → [TotalRuntimeStatus更新]
     ↓                                                                              ↓
[Soubikanri]                                                              [戦闘システムで利用]
```

---

## 2. 関連クラスとその役割

### 2.1 `StatusData` (構造体)
**ファイル**: `Assets\APP\Script\03_Inventory\Chara\StatusData.cs`

**役割**: ステータス情報を保持する構造体

```csharp
[System.Serializable]
public struct StatusData
{
    public int ATK;  // 攻撃力
    public int DEF;  // 防御力
    public int AGI;  // 素早さ
    public int INT;  // 知力
    public int RES;  // 魔法耐性

    // StatusDataの足し算演算子オーバーロード
    public static StatusData operator +(StatusData a, StatusData b)
    {
        return new StatusData
        {
            ATK = a.ATK + b.ATK,
            DEF = a.DEF + b.DEF,
            AGI = a.AGI + b.AGI,
            INT = a.INT + b.INT,
            RES = a.RES + b.RES
        };
    }
}
```

**特徴**:
- 5つのステータスパラメータを1つの構造体にカプセル化
- `+`演算子のオーバーロードにより、2つの`StatusData`を簡単に加算可能

---

### 2.2 `Charadata` (ScriptableObject)
**ファイル**: `Assets\APP\Script\04_ScriptableObject\Charadata\Charadata.cs`

**役割**: キャラクターの基礎ステータスを保持

```csharp
public class Charadata : ScriptableObject 
{
    [SerializeField] private CharaStatus charaStatus;

    // StatusData形式でキャラクターステータスを返す
    public StatusData GetCharaStatus()
    {
        return new StatusData
        {
            ATK = charaStatus.ATK,
            DEF = charaStatus.DEF,
            INT = charaStatus.INT,
            RES = charaStatus.RES,
            AGI = charaStatus.AGI
        };
    }

    // 生のCharaStatusを返す（HP、名前などの情報が必要な場合）
    public CharaStatus GetRawStatus()
    {
        return charaStatus;
    }
}
```

**特徴**:
- ScriptableObjectとしてキャラクターデータを管理
- `GetCharaStatus()`で`StatusData`形式に変換して返す

---

### 2.3 `Soubikanri` (装備管理クラス)
**ファイル**: `Assets\APP\Script\03_Inventory\Soubi\Soubikanri.cs`

**役割**: 装備の管理とUI表示

**重要なメソッド**:

#### `EquipCurrentType(EquipmentData1 eq)` (行278-298)
装備を装着する処理

```csharp
public void EquipCurrentType(EquipmentData1 eq)
{
    // 装備を登録
    equipped[currentType] = eq;
    ApplyToMiddleSlot(currentType, eq);

    // ★ ステータス再計算を呼び出す
    if (statusCalc != null)
    {
        statusCalc.CalculateTotalStatus();  // ← ここで計算開始
        Debug.Log("計算関数を呼び出しました。");
    }
}
```

#### `GetEquippedItems()` (行322-325)
現在装備中のアイテムを返す

```csharp
public Dictionary<EquipmentData1.Equipmenttype, EquipmentData1> GetEquippedItems()
{
    return equipped; // 装備中のDictionaryを返す
}
```

**装備データ構造**:
```csharp
// 装備中テーブル（部位ごとに1つ）
private readonly Dictionary<EquipmentData1.Equipmenttype, EquipmentData1> equipped = new();
```

---

### 2.4 `StatusCalc` (ステータス計算クラス)
**ファイル**: `Assets\APP\Script\03_Inventory\Soubi\StatusCalc.cs`

**役割**: キャラクターステータスと装備ステータスを合算し、最終ステータスを計算

**依存関係**:
```csharp
[SerializeField] private Soubikanri soubiManager;           // 装備管理
[SerializeField] private Charadata charaData;               // キャラデータ
[SerializeField] public CharaStatuskanri statusUI;          // UI更新用
[SerializeField] private TotalRuntimeStatus runtimeStatus;  // 合計ステータス格納
```

#### `Start()` (行16-19)
初期化時にステータスを計算

```csharp
void Start()
{
    CalculateTotalStatus();  // ゲーム開始時に一度計算
}
```

#### `CalculateTotalStatus()` (行21-73) ★核心メソッド
ステータスの合計計算を行う

```csharp
public void CalculateTotalStatus()
{
    // 1. キャラクターの基礎ステータスを取得
    var CharaStatus = charaData.GetCharaStatus();

    // 2. 装備中のアイテムを取得
    var equippedItems = soubiManager.GetEquippedItems();

    // 3. 装備ステータスの合計を初期化
    equipSum = new StatusData();

    // 4. 各装備のステータスを合算
    foreach (var eq in equippedItems.Values)
    {
        if (eq == null)
        {
            Debug.Log("[DEBUG] 装備データが null です。");
            continue;
        }
        
        var s = eq.GetItemStatus();
        equipSum.ATK += s.ATK;
        equipSum.DEF += s.DEF;
        equipSum.AGI += s.AGI;
        equipSum.INT += s.INT;
        equipSum.RES += s.RES;
    }

    // 5. UI更新
    if (statusUI != null)
    {
        statusUI.UpdateStatusDisplay();
    }

    // 6. 最終ステータスを計算（キャラ基礎 + 装備合計）
    StatusData TotalStatus = CharaStatus + equipSum;  // ← 演算子オーバーロード使用
    
    // 7. TotalRuntimeStatusに最終ステータスを設定
    runtimeStatus.SetTotalStatus(TotalStatus);  // ★ここでSetTotalStatusを呼び出し
    Debug.Log("TotalStatusが更新されました");
}
```

**計算式**:
```
最終ステータス = キャラクター基礎ステータス + 装備ステータス合計
```

#### `GetEquipSum()` (行75-78)
装備ステータスの合計を返す（UI表示用）

```csharp
public StatusData GetEquipSum()
{
    return equipSum;
}
```

---

### 2.5 `TotalRuntimeStatus` (最終ステータス保持クラス)
**ファイル**: `Assets\APP\Script\03_Inventory\Soubi\TotalRuntimeStatus.cs`

**役割**: 計算された最終ステータスを保持し、他のシステムから参照可能にする

```csharp
public class TotalRuntimeStatus : MonoBehaviour
{
    // 最終ステータス（読み取り専用プロパティ）
    public StatusData TotalStatus { get; private set; }

    // ★ SetTotalStatusメソッド
    public void SetTotalStatus(StatusData newStatus)
    {
        TotalStatus = newStatus;  // 新しいステータスを設定

        Debug.Log($"[RuntimeStatus] Final Updated: " +
                  $"ATK={TotalStatus.ATK}, DEF={TotalStatus.DEF}, " +
                  $"AGI={TotalStatus.AGI}, INT={TotalStatus.INT}, RES={TotalStatus.RES}");
    }
}
```

**特徴**:
- `TotalStatus`プロパティは外部から読み取り専用（`get`のみ公開）
- `SetTotalStatus`メソッドでのみ更新可能
- 戦闘システムなど他のシステムから`TotalStatus`を参照できる

---

### 2.6 `CharaStatuskanri` (UI表示クラス)
**ファイル**: `Assets\APP\Script\03_Inventory\Chara\CharaStatuskanri.cs`

**役割**: ステータスメニューのUI表示

#### `UpdateStatusDisplay()` (行56-81)
UI更新処理

```csharp
public void UpdateStatusDisplay()
{
    var Status = charadata.GetCharaStatus();      // キャラ基礎ステータス
    var equipSum = statusCalc.GetEquipSum();      // 装備ステータス合計

    // 合計ステータス表示
    totalATKText.text = $"ATK:{Status.ATK + equipSum.ATK}";
    totalDEFText.text = $"DEF:{Status.DEF + equipSum.DEF}";
    // ... 以下同様

    // キャラステータス表示
    basicATKText.text = $"ATK: {Status.ATK}";
    // ... 以下同様

    // 装備ステータス表示
    equipmentATKText.text = $"ATK: {equipSum.ATK}";
    // ... 以下同様
}
```

---

## 3. 呼び出しフロー詳細

### 3.1 ゲーム開始時
```
1. StatusCalc.Start()
   ↓
2. StatusCalc.CalculateTotalStatus()
   ↓
3. TotalRuntimeStatus.SetTotalStatus()
```

### 3.2 装備変更時
```
1. プレイヤーが装備を選択してEボタンを押す
   ↓
2. Soubikanri.EquipCurrentType(eq)
   ├─ equipped[currentType] = eq  (装備を登録)
   └─ statusCalc.CalculateTotalStatus()  (再計算開始)
       ↓
3. StatusCalc.CalculateTotalStatus()
   ├─ charaData.GetCharaStatus()  (キャラステータス取得)
   ├─ soubiManager.GetEquippedItems()  (装備アイテム取得)
   ├─ 装備ステータスを合算 (foreach)
   ├─ TotalStatus = CharaStatus + equipSum  (最終計算)
   ├─ statusUI.UpdateStatusDisplay()  (UI更新)
   └─ runtimeStatus.SetTotalStatus(TotalStatus)  ★ここで呼び出し
       ↓
4. TotalRuntimeStatus.SetTotalStatus(newStatus)
   └─ TotalStatus = newStatus  (最終ステータス保存)
```

---

## 4. ステータス計算の詳細

### 4.1 計算式
```
最終ATK = キャラクター基礎ATK + 装備1のATK + 装備2のATK + ... + 装備NのATK
最終DEF = キャラクター基礎DEF + 装備1のDEF + 装備2のDEF + ... + 装備NのDEF
（以下、AGI、INT、RESも同様）
```

### 4.2 実装における計算手順

#### ステップ1: キャラクター基礎ステータス取得
```csharp
var CharaStatus = charaData.GetCharaStatus();
// 例: { ATK=10, DEF=5, AGI=8, INT=7, RES=6 }
```

#### ステップ2: 装備ステータスの合算
```csharp
equipSum = new StatusData();  // 初期化: 全て0

foreach (var eq in equippedItems.Values)
{
    if (eq == null) continue;
    
    var s = eq.GetItemStatus();
    equipSum.ATK += s.ATK;  // 各装備のATKを加算
    equipSum.DEF += s.DEF;  // 各装備のDEFを加算
    // ... 以下同様
}
// 例: { ATK=15, DEF=10, AGI=5, INT=3, RES=8 }
```

#### ステップ3: 最終ステータス計算
```csharp
StatusData TotalStatus = CharaStatus + equipSum;
// 演算子オーバーロードにより、各パラメータが加算される
// 例: { ATK=25, DEF=15, AGI=13, INT=10, RES=14 }
```

#### ステップ4: 最終ステータスの保存
```csharp
runtimeStatus.SetTotalStatus(TotalStatus);
// TotalRuntimeStatus.TotalStatusに保存される
```

---

## 5. データの流れ図

```
[Charadata (ScriptableObject)]
    ↓ GetCharaStatus()
[StatusData: キャラ基礎]
    ↓
    ├─────────────────┐
    ↓                 ↓
[StatusCalc]    [装備データベース]
    ↓                 ↓
    ↓         [Soubikanri]
    ↓                 ↓ GetEquippedItems()
    ↓         [Dictionary<部位, 装備>]
    ↓                 ↓
    └─→ [CalculateTotalStatus] ←┘
            ↓
        [equipSum計算]
            ↓
        [CharaStatus + equipSum]
            ↓
        [TotalStatus]
            ↓ SetTotalStatus()
    [TotalRuntimeStatus]
            ↓ TotalStatus (プロパティ)
    [戦闘システム等で参照]
```

---

## 6. 重要なポイント

### 6.1 `StatusData`構造体の利点
- **カプセル化**: 5つのパラメータを1つの構造体にまとめることで、管理が容易
- **演算子オーバーロード**: `+`演算子で直感的にステータスを加算可能
- **型安全性**: 構造体として定義することで、型の安全性を確保

### 6.2 `SetTotalStatus`の役割
- **単一責任の原則**: ステータスの設定のみを担当
- **データの保護**: `TotalStatus`プロパティを`private set`にすることで、外部からの直接変更を防止
- **デバッグ**: ステータス更新時にログ出力し、デバッグを容易に

### 6.3 計算タイミング
1. **ゲーム開始時**: `StatusCalc.Start()`で初回計算
2. **装備変更時**: `Soubikanri.EquipCurrentType()`から再計算

### 6.4 UI更新の流れ
```
CalculateTotalStatus()
    ↓
statusUI.UpdateStatusDisplay()
    ↓
CharaStatuskanri が各UIテキストを更新
```

---

## 7. 現在の使用状況

### 7.1 `SetTotalStatus`の呼び出し箇所
- **ファイル**: `StatusCalc.cs`
- **行**: 70
- **コード**: `runtimeStatus.SetTotalStatus(TotalStatus);`

### 7.2 `CalculateTotalStatus`の呼び出し箇所
1. **StatusCalc.Start()** (行18)
   - ゲーム開始時の初期化
   
2. **Soubikanri.EquipCurrentType()** (行286)
   - 装備変更時の再計算

---

## 8. 今後の拡張可能性

### 8.1 戦闘システムでの利用
`CharaDamage.cs`では現在`Charadata`から直接ステータスを取得していますが、`TotalRuntimeStatus`を利用することで装備込みのステータスを反映できます。

**現在の実装**:
```csharp
var Status = charadata.GetCharaStatus();  // 装備が反映されていない
HP -= value - Status.DEF;
```

**改善案**:
```csharp
[SerializeField] private TotalRuntimeStatus totalStatus;

// ダメージ計算時
HP -= value - totalStatus.TotalStatus.DEF;  // 装備込みのDEFを使用
```

### 8.2 バフ・デバフシステムの追加
```csharp
StatusData TotalStatus = CharaStatus + equipSum + buffSum - debuffSum;
```

### 8.3 スキルによるステータス補正
```csharp
StatusData TotalStatus = (CharaStatus + equipSum) * skillMultiplier;
```

---

## 9. まとめ

### システムの特徴
1. **明確な責任分離**: 各クラスが明確な役割を持つ
2. **データの一元管理**: `TotalRuntimeStatus`で最終ステータスを一元管理
3. **柔軟な拡張性**: 新しいステータス計算要素を追加しやすい設計
4. **型安全性**: `StatusData`構造体により型安全な実装

### 呼び出しの流れ（要約）
```
装備変更 → CalculateTotalStatus → ステータス計算 → SetTotalStatus → 保存
```

### キーとなるメソッド
- **`CalculateTotalStatus()`**: ステータス計算のエントリーポイント
- **`SetTotalStatus()`**: 計算結果の保存
- **`StatusData.operator+`**: ステータスの加算処理

このシステムにより、キャラクターの基礎ステータスと装備ステータスが適切に合算され、ゲーム全体で一貫したステータス管理が実現されています。
