namespace Nagiyu.Splatoon3Tracker.Service.Consts
{
    public class Splatoon3Enums
    {
        public enum RecordType
        {
            KillRate
        }

        public enum BattleType
        {
            /// <summary>
            /// レギュラー
            /// </summary>
            Regular,

            /// <summary>
            /// バンカラマッチ (オープン)
            /// </summary>
            AnarchyOpenBattle,

            /// <summary>
            /// バンカラマッチ (チャレンジ)
            /// </summary>
            AnarchySeriesBattle,

            /// <summary>
            /// Xマッチ
            /// </summary>
            XBattle,

            /// <summary>
            /// イベントバトル
            /// </summary>
            Challenge,

            /// <summary>
            /// フェスマッチ (オープン)
            /// </summary>
            SplatfestOpenBattle,

            /// <summary>
            /// フェスマッチ (チャレンジ)
            /// </summary>
            SplatfestProBattle,

            /// <summary>
            /// トリカラアタック
            /// </summary>
            TricolorBattle
        }

        public enum RuleType
        {
            /// <summary>
            /// ナワバリバトル
            /// </summary>
            TurfWar,

            /// <summary>
            /// ガチエリア
            /// </summary>
            SplatZones,

            /// <summary>
            /// ガチヤグラ
            /// </summary>
            TowerControl,

            /// <summary>
            /// ガチホコ
            /// </summary>
            Rainmaker,

            /// <summary>
            /// ガチアサリ
            /// </summary>
            ClamBlitz
        }

        public enum Weapon
        {
            /// <summary>
            /// ボールドマーカー
            /// </summary>
            SplooshOMatic,

            /// <summary>
            /// ボールドマーカーネオ
            /// </summary>
            NeoSplooshOMatic,

            /// <summary>
            /// わかばシューター
            /// </summary>
            SplattershotJr,

            /// <summary>
            /// もみじシューター
            /// </summary>
            CustomSplattershotJr,

            /// <summary>
            /// シャープマーカー
            /// </summary>
            SplashOMatic,

            /// <summary>
            /// シャープマーカーネオ
            /// </summary>
            NeoSplashOMatic,

            /// <summary>
            /// プロモデラーMG
            /// </summary>
            AerosprayMg,

            /// <summary>
            /// プロモデラーRG
            /// </summary>
            AerosprayRg,

            /// <summary>
            /// スプラシューター
            /// </summary>
            Splattershot,

            /// <summary>
            /// スプラシューターコラボ
            /// </summary>
            TentatekSplattershot,

            /// <summary>
            /// ヒーローシューター レプリカ
            /// </summary>
            HeroShotReplica,

            /// <summary>
            /// オクタシューター レプリカ
            /// </summary>
            OctoShotReplica,

            /// <summary>
            /// オーダーシューター レプリカ
            /// </summary>
            OrderShotReplica,

            /// <summary>
            /// .52ガロン
            /// </summary>
            Gal52,

            /// <summary>
            /// .52ガロンデコ
            /// </summary>
            Gal52Deco,

            /// <summary>
            /// N-ZAP85
            /// </summary>
            NZap85,

            /// <summary>
            /// N-ZAP89
            /// </summary>
            NZap89,

            /// <summary>
            /// プライムシューター
            /// </summary>
            SplattershotPro,

            /// <summary>
            /// プライムシューターコラボ
            /// </summary>
            ForgeSplattershotPro,

            /// <summary>
            /// .96ガロン
            /// </summary>
            Gal96,

            /// <summary>
            /// .96ガロンデコ
            /// </summary>
            Gal96Deco,

            /// <summary>
            /// ジェットスイーパー
            /// </summary>
            JetSquelcher,

            /// <summary>
            /// ジェットスイーパーカスタム
            /// </summary>
            CustomJetSquelcher,

            /// <summary>
            /// スペースシューター
            /// </summary>
            SplattershotNova,

            /// <summary>
            /// スペースシューターコラボ
            /// </summary>
            AnnakiSplattershotNova,

            /// <summary>
            /// L3リールガン
            /// </summary>
            L3Nozzlenose,

            /// <summary>
            /// L3リールガンD
            /// </summary>
            L3NozzlenoseD,

            /// <summary>
            /// H3リールガン
            /// </summary>
            H3Nozzlenose,

            /// <summary>
            /// H3リールガンD
            /// </summary>
            H3NozzlenoseD,

            /// <summary>
            /// ボトルガイザー
            /// </summary>
            Squeezer,

            /// <summary>
            /// ボトルガイザーフォイル
            /// </summary>
            FoilSqueezer,

            /// <summary>
            /// カーボンローラー
            /// </summary>
            CarbonRoller,

            /// <summary>
            /// カーボンローラーデコ
            /// </summary>
            CarbonRollerDeco,

            /// <summary>
            /// スプラローラー
            /// </summary>
            SplatRoller,

            /// <summary>
            /// スプラローラーコラボ
            /// </summary>
            KrakOnSplatRoller,

            /// <summary>
            /// オーダーローラー レプリカ
            /// </summary>
            OrderRollerReplica,

            /// <summary>
            /// ダイナモローラー
            /// </summary>
            DynamoRoller,

            /// <summary>
            /// ダイナモローラーテスラ
            /// </summary>
            GoldDynamoRoller,

            /// <summary>
            /// ヴァリアブルローラー
            /// </summary>
            FlingzaRoller,

            /// <summary>
            /// ヴァリアブルローラーフォイル
            /// </summary>
            FoilFlingzaRoller,

            /// <summary>
            /// ワイドローラー
            /// </summary>
            BigSwigRoller,

            /// <summary>
            /// ワイドローラーコラボ
            /// </summary>
            BigSwigRollerExpress,

            /// <summary>
            /// スクイックリンα
            /// </summary>
            ClassicSquiffer,

            /// <summary>
            /// スクイックリンβ
            /// </summary>
            NewSquiffer,

            /// <summary>
            /// スプラチャージャー
            /// </summary>
            SplatCharger,

            /// <summary>
            /// スプラチャージャーコラボ
            /// </summary>
            ZFSplatCharger,

            /// <summary>
            /// オーダーチャージャー レプリカ
            /// </summary>
            OrderChargerReplica,

            /// <summary>
            /// スプラスコープ
            /// </summary>
            Splatterscope,

            /// <summary>
            /// スプラスコープコラボ
            /// </summary>
            ZFSplatterscope,

            /// <summary>
            /// リッター4K
            /// </summary>
            ELiter4K,

            /// <summary>
            /// リッター4Kカスタム
            /// </summary>
            CustomELiter4K,

            /// <summary>
            /// 4Kスコープ
            /// </summary>
            ELiter4KScope,

            /// <summary>
            /// 4Kスコープカスタム
            /// </summary>
            CustomELiter4KScope,

            /// <summary>
            /// 14式竹筒銃・甲
            /// </summary>
            Bamboozler14MkI,

            /// <summary>
            /// 14式竹筒銃・乙
            /// </summary>
            Bamboozler14MkIi,

            /// <summary>
            /// ソイチューバー
            /// </summary>
            GooTuber,

            /// <summary>
            /// ソイチューバーカスタム
            /// </summary>
            CustomGooTuber,

            /// <summary>
            /// R-PEN/5H
            /// </summary>
            Snipewriter5H,

            /// <summary>
            /// R-PEN/5B
            /// </summary>
            Snipewriter5B,

            /// <summary>
            /// バケットスロッシャー
            /// </summary>
            Slosher,

            /// <summary>
            /// バケットスロッシャーデコ
            /// </summary>
            SlosherDeco,

            /// <summary>
            /// オーダースロッシャー レプリカ
            /// </summary>
            OrderSlosherReplica,

            /// <summary>
            /// ヒッセン
            /// </summary>
            TriSlosher,

            /// <summary>
            /// ヒッセン・ヒュー
            /// </summary>
            TriSlosherNouveau,

            /// <summary>
            /// スクリュースロッシャー
            /// </summary>
            SloshingMachine,

            /// <summary>
            /// スクリュースロッシャーネオ
            /// </summary>
            SloshingMachineNeo,

            /// <summary>
            /// オーバーフロッシャー
            /// </summary>
            Bloblobber,

            /// <summary>
            /// オーバーフロッシャーデコ
            /// </summary>
            BloblobberDeco,

            /// <summary>
            /// エクスプロッシャー
            /// </summary>
            Explosher,

            /// <summary>
            /// エクスプロッシャーカスタム
            /// </summary>
            CustomExplosher,

            /// <summary>
            /// モップリン
            /// </summary>
            DreadWringer,

            /// <summary>
            /// モップリンD
            /// </summary>
            DreadWringerD,

            /// <summary>
            /// スプラスピナー
            /// </summary>
            MiniSplatling,

            /// <summary>
            /// スプラスピナーコラボ
            /// </summary>
            ZinkMiniSplatling,

            /// <summary>
            /// バレルスピナー
            /// </summary>
            HeavySplatling,

            /// <summary>
            /// バレルスピナーデコ
            /// </summary>
            HeavySplatlingDeco,

            /// <summary>
            /// オーダースピナー レプリカ
            /// </summary>
            OrderSplatlingReplica,

            /// <summary>
            /// ハイドラント
            /// </summary>
            HydraSplatling,

            /// <summary>
            /// ハイドラントカスタム
            /// </summary>
            CustomHydraSplatling,

            /// <summary>
            /// クーゲルシュライバー
            /// </summary>
            BallpointSplatling,

            /// <summary>
            /// クーゲルシュライバー・ヒュー
            /// </summary>
            BallpointSplatlingNouveau,

            /// <summary>
            /// ノーチラス47
            /// </summary>
            Nautilus47,

            /// <summary>
            /// ノーチラス79
            /// </summary>
            Nautilus79,

            /// <summary>
            /// イグザミナー
            /// </summary>
            HeavyEditSplatling,

            /// <summary>
            /// イグザミナー・ヒュー
            /// </summary>
            HeavyEditSplatlingNouveau,

            /// <summary>
            /// スパッタリー
            /// </summary>
            DappleDualies,

            /// <summary>
            /// スパッタリー・ヒュー
            /// </summary>
            DappleDualiesNouveau,

            /// <summary>
            /// スプラマニューバー
            /// </summary>
            SplatDualies,

            /// <summary>
            /// スプラマニューバーコラボ
            /// </summary>
            EnperrySplatDualies,

            /// <summary>
            /// オーダーマニューバー レプリカ
            /// </summary>
            OrderDualieReplicas,

            /// <summary>
            /// ケルビン525
            /// </summary>
            GloogaDualies,

            /// <summary>
            /// ケルビン525デコ
            /// </summary>
            GloogaDualiesDeco,

            /// <summary>
            /// デュアルスイーパー
            /// </summary>
            DualieSquelchers,

            /// <summary>
            /// デュアルスイーパーカスタム
            /// </summary>
            CustomDualieSquelchers,

            /// <summary>
            /// クアッドホッパーブラック
            /// </summary>
            DarkTetraDualies,

            /// <summary>
            /// クアッドホッパーホワイト
            /// </summary>
            LightTetraDualies,

            /// <summary>
            /// ガエンFF
            /// </summary>
            DouserDualiesFf,

            /// <summary>
            /// ガエンFFカスタム
            /// </summary>
            CustomDouserDualiesFf,

            /// <summary>
            /// パラシェルター
            /// </summary>
            SplatBrella,

            /// <summary>
            /// パラシェルターソレーラ
            /// </summary>
            SorellaBrella,

            /// <summary>
            /// オーダーシェルター レプリカ
            /// </summary>
            OrderBrellaReplica,

            /// <summary>
            /// キャンピングシェルター
            /// </summary>
            TentaBrella,

            /// <summary>
            /// キャンピングシェルターソレーラ
            /// </summary>
            TentaSorellaBrella,

            /// <summary>
            /// スパイガジェット
            /// </summary>
            UndercoverBrella,

            /// <summary>
            /// スパイガジェットソレーラ
            /// </summary>
            UndercoverSorellaBrella,

            /// <summary>
            /// 24式張替傘・甲
            /// </summary>
            RecycledBrella24MkI,

            /// <summary>
            /// 24式張替傘・乙
            /// </summary>
            RecycledBrella24MkIi,

            /// <summary>
            /// ノヴァブラスター
            /// </summary>
            LunaBlaster,

            /// <summary>
            /// ノヴァブラスターネオ
            /// </summary>
            LunaBlasterNeo,

            /// <summary>
            /// オーダーブラスター レプリカ
            /// </summary>
            OrderBlasterReplica,

            /// <summary>
            /// ホットブラスター
            /// </summary>
            Blaster,

            /// <summary>
            /// ホットブラスターカスタム
            /// </summary>
            CustomBlaster,

            /// <summary>
            /// ロングブラスター
            /// </summary>
            RangeBlaster,

            /// <summary>
            /// ロングブラスターカスタム
            /// </summary>
            CustomRangeBlaster,

            /// <summary>
            /// クラッシュブラスター
            /// </summary>
            ClashBlaster,

            /// <summary>
            /// クラッシュブラスターネオ
            /// </summary>
            ClashBlasterNeo,

            /// <summary>
            /// ラピッドブラスター
            /// </summary>
            RapidBlaster,

            /// <summary>
            /// ラピッドブラスターデコ
            /// </summary>
            RapidBlasterDeco,

            /// <summary>
            /// Rブラスターエリート
            /// </summary>
            RapidBlasterPro,

            /// <summary>
            /// Rブラスターエリートデコ
            /// </summary>
            RapidBlasterProDeco,

            /// <summary>
            /// S-BLAST92
            /// </summary>
            SBlast92,

            /// <summary>
            /// S-BLAST91
            /// </summary>
            SBlast91,

            /// <summary>
            /// パブロ
            /// </summary>
            Inkbrush,

            /// <summary>
            /// パブロ・ヒュー
            /// </summary>
            InkbrushNouveau,

            /// <summary>
            /// ホクサイ
            /// </summary>
            Octobrush,

            /// <summary>
            /// ホクサイ・ヒュー
            /// </summary>
            OctobrushNouveau,

            /// <summary>
            /// オーダーブラシ レプリカ
            /// </summary>
            OrderbrushReplica,

            /// <summary>
            /// フィンセント
            /// </summary>
            Painbrush,

            /// <summary>
            /// フィンセント・ヒュー
            /// </summary>
            PainbrushNouveau,

            /// <summary>
            /// トライストリンガー
            /// </summary>
            TriStringer,

            /// <summary>
            /// トライストリンガーコラボ
            /// </summary>
            InklineTriStringer,

            /// <summary>
            /// オーダーストリンガー レプリカ
            /// </summary>
            OrderStringerReplica,

            /// <summary>
            /// LACT-450
            /// </summary>
            ReefLux450,

            /// <summary>
            /// LACT-450デコ
            /// </summary>
            ReefLux450Deco,

            /// <summary>
            /// フルイドV
            /// </summary>
            WellstringV,

            /// <summary>
            /// フルイドVカスタム
            /// </summary>
            CustomWellstringV,

            /// <summary>
            /// ジムワイパー
            /// </summary>
            SplatanaStamper,

            /// <summary>
            /// ジムワイパー・ヒュー
            /// </summary>
            SplatanaStamperNouveau13,

            /// <summary>
            /// オーダーワイパー レプリカ
            /// </summary>
            OrderSplatanaReplica,

            /// <summary>
            /// ドライブワイパー
            /// </summary>
            SplatanaWiper,

            /// <summary>
            /// ドライブワイパーデコ
            /// </summary>
            SplatanaWiperDeco,

            /// <summary>
            /// デンタルワイパーミント
            /// </summary>
            MintDecavitator,

            /// <summary>
            /// デンタルワイパースミ
            /// </summary>
            CharcoalDecavitator
        }

        public enum ResultType
        {
            Win,
            Lose
        }
    }
}
