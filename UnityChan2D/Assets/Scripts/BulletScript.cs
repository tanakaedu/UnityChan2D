using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
	private GameObject player;
	private int speed = 10;


	// Use this for initialization
	void Start () {
		// ユニティちゃんオブジェクトを取得
		player = GameObject.FindWithTag("UnityChan");
		// rididbody2Dコンポーネントを取得
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		// ユニティちゃんの向きに弾を飛ばす
		rb.velocity = new Vector2(speed * player.transform.localScale.x, rb.velocity.y);
		// 画像の向きをユニティちゃんにあわせる
		Vector2 temp = transform.localScale;
		temp.x = player.transform.localScale.x;
		transform.localScale = temp;
		// 5秒後に消滅
		Destroy(gameObject, 5f);
	}
	
}
