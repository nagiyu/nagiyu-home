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
      "UserName": {"S": "なぎゆー"},
      "OneSignalSubscriptionId": {"S": "1579fb81-05c6-45b9-9f4d-c0c3236ceba4"}
    }'

# DynamoDB テーブル「Splatoon3Tracker」を作成
awslocal dynamodb create-table \
  --table-name Splatoon3Tracker \
  --attribute-definitions \
    AttributeName=Id,AttributeType=S \
    AttributeName=RecordType,AttributeType=S \
  --key-schema \
    AttributeName=Id,KeyType=HASH \
  --global-secondary-indexes \
    '[
      {
        "IndexName": "KillRate-index-20250112",
        "KeySchema": [
          { "AttributeName": "RecordType", "KeyType": "HASH" }
        ],
        "Projection": {
          "ProjectionType": "INCLUDE",
          "NonKeyAttributes": ["UserId", "Battle", "Rule", "Weapon", "Result", "Kill", "Assist", "Death", "Special", "Date", "MatchTime"]
        }
      }
    ]' \
  --billing-mode PAY_PER_REQUEST \
  --table-class STANDARD \
  --sse-specification Enabled=true,SSEType=AES256
