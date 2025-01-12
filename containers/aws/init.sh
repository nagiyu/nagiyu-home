#!/bin/bash

# DynamoDB テーブル「Auth」を作成
awslocal dynamodb create-table \
  --table-name Auth \
  --attribute-definitions \
    AttributeName=UserId,AttributeType=S \
    AttributeName=GoogleUserId,AttributeType=S \
  --key-schema \
    AttributeName=UserId,KeyType=HASH \
  --global-secondary-indexes \
    '[
      {
        "IndexName": "GoogleUserId-index-20250112",
        "KeySchema": [
          { "AttributeName": "GoogleUserId", "KeyType": "HASH" }
        ],
        "Projection": {
          "ProjectionType": "INCLUDE",
          "NonKeyAttributes": ["UserId", "UserName"]
        }
      }
    ]' \
  --billing-mode PAY_PER_REQUEST \
  --table-class STANDARD \
  --sse-specification Enabled=true,SSEType=AES256

# DynamoDB の Auth にデータ追加
awslocal dynamodb put-item \
  --table-name Auth \
  --item \
    '{
      "UserId": {"S": "acaa64d6-3da6-49a7-9c52-d57cfe8bdb34"},
      "GoogleUserId": {"S": "105305940240519833154"},
      "UserName": {"S": "なぎゆー"}
      "Role_Splatoon3Tracker": {"S": "User"}
    }'

# DynamoDB テーブル「Splatoon3Tracker」を作成
awslocal dynamodb create-table \
  --table-name Splatoon3Tracker \
  --attribute-definitions \
    AttributeName=Id,AttributeType=S \
    AttributeName=Type,AttributeType=S \
  --key-schema \
    AttributeName=Id,KeyType=HASH \
  --global-secondary-indexes \
    '[
      {
        "IndexName": "KillRate-index-20250112",
        "KeySchema": [
          { "AttributeName": "Type", "KeyType": "HASH" }
        ],
        "Projection": {
          "ProjectionType": "INCLUDE",
          "NonKeyAttributes": ["Battle", "Rule", "Weapon", "Result", "Kill", "Assist", "Death", "Special", "Date", "MatchTime"]
        }
      }
    ]' \
  --billing-mode PAY_PER_REQUEST \
  --table-class STANDARD \
  --sse-specification Enabled=true,SSEType=AES256

# DynamoDB の Splatoon3Tracker にデータ追加
awslocal dynamodb put-item \
  --table-name Splatoon3Tracker \
  --item \
    '{
      "Id": {"S": "sample-id-1"},
      "Type": {"S": "KillRate"},
      "Battle": {"S": "Xマッチ"},
      "Rule": {"S": "ガチアサリ"},
      "Weapon": {"S": "R-PEN/5H"},
      "Result": {"S": "Lose"},
      "Kill": {"N": "5"},
      "Assist": {"N": "1"},
      "Death": {"N": "6"},
      "Special": {"N": "6"},
      "Date": {"S": "2025-01-05T19:49:00Z"},
      "MatchTime": {"S": "5:00"}
    }'

awslocal dynamodb put-item \
  --table-name Splatoon3Tracker \
  --item \
    '{
      "Id": {"S": "sample-id-2"},
      "Type": {"S": "KillRate"},
      "Battle": {"S": "バンカラマッチ"},
      "Rule": {"S": "ガチホコ"},
      "Weapon": {"S": "R-PEN/5H"},
      "Result": {"S": "Lose"},
      "Kill": {"N": "0"},
      "Assist": {"N": "0"},
      "Death": {"N": "3"},
      "Special": {"N": "1"},
      "Date": {"S": "2025-01-05T20:50:00Z"},
      "MatchTime": {"S": "1:08"}
    }'

awslocal dynamodb put-item \
  --table-name Splatoon3Tracker \
  --item \
    '{
      "Id": {"S": "sample-id-3"},
      "Type": {"S": "KillRate"},
      "Battle": {"S": "レギュラーマッチ"},
      "Rule": {"S": "ナワバリバトル"},
      "Weapon": {"S": "イグザミナー"},
      "Result": {"S": "Lose"},
      "Kill": {"N": "1"},
      "Assist": {"N": "0"},
      "Death": {"N": "3"},
      "Special": {"N": "2"},
      "Date": {"S": "2025-01-11T14:57:00Z"},
      "MatchTime": {"S": "3:00"}
    }'
