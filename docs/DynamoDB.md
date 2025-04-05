# DynamoDB

- DynamoDB の扱いについて記述する。

## 構成

![](./assets/DynamoDB/Container.drawio.svg)

- 各 Service から直接 DynamoDB にアクセスするのではなく、DynamoDBService を経由する

- DynamoDBService は DynamoDB のテーブルごとに作成する

- DynamoDBService と DynamoDB のやり取りは Item のクラスを使用し、各 Service と DynamoDBService のやり取りは Model のクラスを使用する

### DynamoDB

```mermaid
erDiagram
    HogeItem {
        string Id
        string ItemType
        string Column1
        string CreatedAt
        string UpdatedAt
    }

    SystemItem {
        string Id
        string ItemType
        string SystemKey
        string SystemValue
        string CreatedAt
        string UpdatedAt
    }
```

- 各レコードは HogeItem に格納する

    - ItemType は `Normal`

- SystemItem は全テーブルに作成する

    - ItemType は `System`

    - 必須となるレコードは下記

        - Key: Counter

            - HogeItem の Id を管理する

            - DynamoDB はオートインクリメントが無いため

### Item

```mermaid
classDiagram
class ItemBase {
    + string Id
    + string ItemType
    + string CreatedAt
    + string UpdatedAt
    + ItemBase()
    + ItemBase(Dictionary~string, AttributeValue~ keyValuePairs)
}

class SystemItem {
    + string ItemType = "System"
    + SystemKey
    + SystemValue
    + SystemItem()
    + SystemItem(Dictionary~string, AttributeValue~ keyValuePairs)
}

ItemBase <|-- SystemItem

class HogeItem {
    + string ItemType = "Normal"
    + HogeItem()
    + HogeItem(Dictionary~string, AttributeValue~ keyValuePairs)
}

ItemBase <|-- HogeItem
```

### DynamoDBService

```mermaid
classDiagram
class DynamoDBServiceBase {
    <<Abstract>>
    # string TableName
    - AmazonDynamoDBClient client
    + DynamoDBServiceBase(IConfiguration configuration)
    + Task~T~ GetItem~T~(Guid id)
    + Task<(List~T~, Guid?)> GetItems~T~(Dictionary~string, string~ expressions, Guid? startId)
    + Task~Guid~ AddItem~T~(T Item)
    + Task UpdateItem~T~(Guid id, T Item)
    + Task DeleteItem(Guid id)
}

class HogeDBService {
    # string = "Hoge"
}

DynamoDBServiceBase <|-- HogeDBService
