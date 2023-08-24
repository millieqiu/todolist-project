# TODO LIST 🗓

## 作品說明
一款具備登入 / 註冊功能的簡易 Todo List 應用程式 <br>
輕鬆管理您的待辦事項 📋

## 畫面
專案初期，利用 Figma 繪製 wireframe 確認需求與功能可行性。<br>
繪製介面設計精稿，製作可重複使用的元件，便於後續切版及主視覺維護與更新。<br>
[Figma 連結](https://www.figma.com/file/J5xkKXjdKPLVt1cU9PSth1/TO-DO-LIST-(Practice)?type=design&node-id=0%3A1&mode=design&t=UYA6aB8FdkrGe6Jn-1)

## 資料夾說明
- BuilderServices - 存放 IServiceCollection 的擴充方法
- Controllers - API 及流程控制
- Middlewares - 存放自定義中介軟體
- Migrations - 存放 CodeFirst 產生的 Migration 紀錄
- Models - 定義核心模型
- Pages - 畫面放置處
- RequestModel - 定義 HTTP Request 的表單結構
- Services - 存放 business logic
- Src - 靜態資源放置處
    - css - scss 檔案放置處
    - image - 圖片放置處
- ViewModel - 定義 HTTP Response 的結構

## 專案技術
1. `Vue.js`：透過 Vue.js 實作資料綁定與操作網頁上的元件，將賦值、渲染與事件交由 Vue 處理，替換傳統 `document.getElementById` 等用法。
2. `Webpack`：將眾多模組與資源打包成一包檔案，並編譯我們需要預先處理的內容，變成瀏覽器看得懂的東西，讓我們可以上傳到伺服器。
3. `Bootstrap`：使用內建樣式替換前端元件，快速設計及自定義響應式網站，幫助我們處理 hover 等動態效果。
4. `SASS`：CSS 預處理器，利用變數 (Variables) 設定色碼，方便我們管理網站的色彩樣式，提升效率。
5. `Azure DevOps`：協助我們管理專案進度及管理程式碼版本，讓我們有更多餘力專注在開發上。
6. `ASP.NET Core`：使用 ASP.NET Core 建立 WebAPI 及商業邏輯
7. `MSSQL`：建立穩定易用的關連式資料庫


## 第三方服務
1. `Google OAuth 2.0`：實作使用 Google 帳戶登入，使用授權碼驗證流程，獲得權限並存取使用者的信箱與姓名。

## 現有功能
#### 1️⃣ 登入 / 註冊頁
- 一般登入
    - 錯誤發生時抓取錯誤訊息並彈出提示框
- 第三方登入
    - 利用 Google 帳號登入使用此應用程式
- 註冊
    - 驗證註冊表單中欄位的正確性
    - 利用正則表達式檢驗輸入格式，並同步給予 User 回饋
- Loading Status
    - 進行非同步操作時，加入 Loading 動畫提升使用者體驗

#### 2️⃣ 待辦事項頁
- 新增 Todo task
    - 限制最大輸入字數 30 字
    - 已輸入字數即時提示功能
- 刪除 Todo task
- 完成 Todo task
- 分頁
    - 每頁筆數最多 10 筆，超過便會自動分頁

#### 3️⃣ 使用者設定頁
- 預設圖片
    - User 未設定大頭照前，給定一張預設圖片
- 選擇檔案
    - 選擇照片後即時預覽
    - 在上傳照片前可持續挑選照片
- 進度條
    - 利用 `FileReader` 製作圖片讀取時的進度條
- 上傳大頭照

#### 4️⃣ 資料庫設計
- 使用者資料表
- 待辦事項資料表
- 列舉角色型別（一般使用者及管理員）
- 限制資料表欄位長度
- 建立非叢集索引
- 補正模型或功能異動後產生的髒資料
- 依照先後順序，將最後新增的資料排在最上面

#### 5️⃣ API 及核心服務設計
- 使用者
    - 註冊時檢查使用者帳號是否存在
    - 註冊時檢查確認密碼的正確性
    - 密碼使用 Pbkdf2 加鹽進行雜湊
    - 使用 claim 紀錄角色等不常變動的使用者資訊，減少查詢，減輕資料庫負擔
    - 限制使用者照片類型不能是圖片以外的類型
- 待辦事項
    - 待辦事項的增刪功能
    - 可更改待辦事項的狀態（已勾選/未勾選）
    - 待辦事項列表分頁功能：使用 Skip 及 Take 方法實作分頁
    - 一般使用者僅會顯示登入者建立的待辦事項
    - 管理員可查詢指定使用者建立的待辦事項
- 外部服務
    - 中央氣象局
        - 新增清單項目時寫入天氣資訊
        - 使用 memory-cache 紀錄天氣資訊，避免 API 一直被呼叫
        - 考慮天氣最長字數，設定前端元件大小


#### 6️⃣ Webpack 打包
- npm 安裝外部套件
- `babel-loader` 將最新的語法轉為最相容的版本
- 打包 scss 為 css
- 將 webpack 切分為 dev 和 prod 兩個環境
- 使用 webpack 的 watch 即時追蹤變更，讓 dev 環境可以自動 build
- production 環境設定將程式碼 minify
- 加入 sourcemap，便於除錯

#### 7️⃣ 架構及優化
- 效能
    - 使用 `RenderSection` 調整 css 及 js 檔的載入時機
- Log
    - 使用 NLog 管理 Log 資料
- 設計模式
    - 使用 DI 建立服務
        - 降低系統耦合度
        - 可將服務注入需要的地方使用
- 文件
    - 使用 NSwag 自動產生 API 文件

## 開發紀錄
- [功能異動文件]()

## 待開發
- [ ] 編輯已存在的 Todo task
- [ ] 使用 tag 分類
- [ ] 多人共同編輯
- [ ] 使用拖曳更改清單排序功能
- [ ] 設定到期日及提醒

## 聯絡作者
如果對這份專案感興趣，或有任何問題與指教，歡迎透過以下方式與我們聯繫：
- [Vincent Lu - LinkedIn](https://www.linkedin.com/in/vincent87720/)
- [Millie Qiu - LinkedIn](https://www.linkedin.com/in/qiumillie/)

## Reference
> [Vue.js](https://vuejs.org/) - Vue.js 官方文件，學習此套框架的屬性與用法。<br>
> [Webpack - production](https://webpack.js.org/guides/production/) - 學習如何配置開發環境與正式環境。<br>
> [WebAPIs Fetch API - PJCHENder](https://pjchender.dev/webapis/webapis-fetch/)

