# VVVF Simulator
PC上で、VVVFの音を再現します。<br>

# 使い方
このプログラムは、VisualStudio上のC#コンソールアプリ用です。<br>

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

## マスコン状況出力
このアプリケーションは、マスコンの操作状況を動画で出力できます。形式は .avi です。<br>

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
