# VVVF Simulator
PC上で、VVVFの音を再現します。<br>

# 使い方
このプログラムは、VisualStudio上のC#コンソールアプリ用です。<br>

# 使用上の注意
このプログラムを使用した　作品(動画や、解説動画）等は作ることを歓迎します。<br>
しかしながら、次の点は必ず行ってください。<br>
- このGitHubのURLを貼り付ける。

次の点はしないでください<br>
- このコードを改造して、どこかに公開する。（ただし、VVVF音のコードは許可する）
- このコードを他人に配布する。

# 依存関係

## 動画の出力
・OpenCV - Nugetから、取得できます。<br>
・System.Drawing.Common - Nugetから、取得できます。<br>
・OpenH264 - ネットから取得できます。<br>

### openH264 とは
こちらのURLから、ダウンロードできます。<br>
https://github.com/cisco/openh264/releases<br>
このアプリで使用するバージョンは 1.8.0 です。<br>
ファイル名は`openh264-1.8.0-win64.dll.bz2`のようになっています。<br>
ダウンロードした後、展開し、`openh264-1.8.0-win64.dll`を、実行ファイルと同じディレクトリに設置します。<br>

## 音声の出力
・特に依存関係はありません。

## リアルタイム音声生成
・NAudio - Nugetから、取得できます。

# 機能
## VVVF音声出力
このアプリケーションは、再現された音声データを wav 形式で出力します。<br>
特に変更がなければ、 192kHz でサンプリングされた wav ファイルになります。<br>

## 波形動画出力
このアプリケーションは、VVVFの波形を動画で出力できます。形式は .avi です。<br>
![2022-02-14](https://user-images.githubusercontent.com/77259842/153803020-6615bcce-22a6-4839-b919-ea114dc12d03.png)

## 電圧ベクトル動画出力
このアプリケーションは、例の六角形を動画で出力できます。形式は .avi です。<br>
![2022-02-14 (1)](https://user-images.githubusercontent.com/77259842/153803078-11798e5b-8d68-4a32-9d3b-4f954b8e910c.png)

## マスコン状況出力
このアプリケーションは、マスコンの操作状況を動画で出力できます。形式は .avi です。<br>
シンプルなものと、たろいも氏風があります。<br>
![2022-02-13_7](https://user-images.githubusercontent.com/77259842/153803111-5ad44bb2-2531-48fb-95b4-8b9732fe59c9.png)
![2022-02-14 (3)](https://user-images.githubusercontent.com/77259842/153803208-18692183-b1ae-4251-96dc-ccc4ce8b3c10.png)

## リアルタイム音声生成
いろいろ、リアルタイムで遊べます。<br>
キー操作<br>
```
W - 変化大
S - 変化中
X - 変化小
B - ブレーキON/OFF
N - マスコンON/OFF
R - VVVF音再選択
Enter - 終了
```

# 親プロジェクト
このプログラムは、Raspberry pi zero vvvf から派生しました。
https://github.com/JOTAN-0655/RPi-Zero-VVVF
