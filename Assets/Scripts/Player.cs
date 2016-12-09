using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed = 4f; //歩くスピード
	public GameObject mainCamera;
	private Rigidbody2D rigidbody2D;
	private Animator anim;
	public float scrOffset = 4f;
	public float jumpPower = 700f;	// ジャンプ力
	public LayerMask groundLayer;	// Linecastで判定するLayer
	private bool isGrounded;		// 着地判定

	void Start () {
		//各コンポーネントをキャッシュしておく
		anim = GetComponent<Animator>();
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	void Update() {
		// Linecastでユニティちゃんの足元に地面があるかを判定
		isGrounded = Physics2D.Linecast(
			transform.position+transform.up*1,
			transform.position-transform.up*0.05f,
			groundLayer
		);

		// スペースキーチェック
		if (Input.GetKeyDown ("space")) {
			// 着地していたら
			if (isGrounded) {
				// Dashアニメをとめてジャンプへ
				anim.SetBool("Dash", false);
				anim.SetTrigger ("Jump");
				// 着地フラグをfalse
				isGrounded = false;
				// 上へ加速
				rigidbody2D.AddForce(Vector2.up * jumpPower);
			}
		}

		// 上下への移動速度を取得
		float velY = rigidbody2D.velocity.y;
		// 移動速度が0.1より大きければ上昇
		bool isJumping = velY > 0.1f;
		// 移動速度が-0.1より小さければ下降
		bool isFalling = velY < -0.1f;
		// 結果をアニメータービューの変数に反映
		anim.SetBool("isJumping", isJumping);
		anim.SetBool ("isFalling", isFalling);
	}

	void FixedUpdate ()
	{
		//左キー: -1、右キー: 1
		float x = Input.GetAxisRaw ("Horizontal");
		//左か右を入力したら
		if (x != 0) {
			//入力方向へ移動
			rigidbody2D.velocity = new Vector2 (x * speed, rigidbody2D.velocity.y);
			//localScale.xを-1にすると画像が反転する
			Vector2 temp = transform.localScale;
			temp.x = x;
			transform.localScale = temp;
			//Wait→Dash
			anim.SetBool ("Dash", true);

			//左も右も入力していなかったら
		} else {
			//横移動の速度を0にしてピタッと止まるようにする
			rigidbody2D.velocity = new Vector2 (0, rigidbody2D.velocity.y);
			//Dash→Wait
			anim.SetBool ("Dash", false);
		}

		// 画面中央から左にscrOffset移動した位置をユニティちゃんが超えたら
		if (rigidbody2D.transform.position.x > mainCamera.transform.position.x - scrOffset) {
			// カメラの位置を取得
			Vector3 cameraPos = mainCamera.transform.position;
			// カメラの位置をUnityちゃん中心に移動
			cameraPos.x = rigidbody2D.transform.position.x + scrOffset;
			mainCamera.transform.position = cameraPos;
		}

		// カメラ表示領域の左下をワールド座標に変換
		Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0,0));
		// カメラ表示座標の右上をワールド座標に変換
		Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1,1));
		// Unityちゃんの座標を取得
		Vector2 pos = rigidbody2D.transform.position;
		// Unityちゃんの移動範囲を抑制
		pos.x = Mathf.Clamp(pos.x, min.x+0.5f,max.x);
		rigidbody2D.transform.position = pos;


	}
}
