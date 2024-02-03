using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PowerupPanelScript : MonoBehaviour
{
    private PowerupTypes leftPowerupType;
    private PowerupTypes rightPowerupType;

    [SerializeField] private TextMeshProUGUI leftBuffName;
    [SerializeField] private TextMeshProUGUI leftBuffQuote;
	[SerializeField] private Image leftBuffImage;
	[SerializeField] private TextMeshProUGUI leftBuffDesc;

	[SerializeField] private TextMeshProUGUI rightBuffName;
	[SerializeField] private TextMeshProUGUI rightBuffQuote;
    [SerializeField] private Image rightBuffImage;
	[SerializeField] private TextMeshProUGUI rightBuffDesc;

	[SerializeField] private Sprite bananaSprite;
    [SerializeField] private Sprite cucumberSprite;
	[SerializeField] private Sprite bokChoySprite;
	[SerializeField] private Sprite plumSprite;
	[SerializeField] private Sprite chiliSprite;
	[SerializeField] private Sprite pepperSprite;
	[SerializeField] private Sprite grapeSprite;
	[SerializeField] private Sprite appleSprite;
	[SerializeField] private Sprite carrotSprite;
	[SerializeField] private Sprite berrySprite;
	[SerializeField] private Sprite orangeSprite;
	[SerializeField] private Sprite kiwiSprite;
	[SerializeField] private Sprite broccaliSprite;

	private PowerupScripts powerupScripts;
    private Action OnComplete;

    public void Init(PowerupTypes left, PowerupTypes right, PowerupScripts ps, Action onComplete = null)
    {
        leftPowerupType = left;
        rightPowerupType = right;

        SetupPowerup(leftPowerupType, leftBuffName, leftBuffQuote, leftBuffDesc, leftBuffImage);
        SetupPowerup(rightPowerupType, rightBuffName, rightBuffQuote, rightBuffDesc, rightBuffImage);

        powerupScripts = ps;

		OnComplete = onComplete;

        gameObject.SetActive(true);
	}

    private void SetupPowerup(PowerupTypes type, TextMeshProUGUI title, TextMeshProUGUI quote, TextMeshProUGUI desc, Image image)
    {
        switch (type)
        {
            case PowerupTypes.Banana:
                title.text = "BANANAPOLEON";
                quote.text = "\"There is nothing we can peel.\"";
                image.sprite = bananaSprite;
                desc.text = "Bananapoleon will rotate around you, dealing damage to foes he hits!";
                break;

			case PowerupTypes.Cucumber:
				title.text = "HERCUCUMBER";
				quote.text = "\"I can go the diLLstance.\"";
				image.sprite = cucumberSprite;
				desc.text = "Hercucumber will give you his strength, increasing your damage!";
				break;

			case PowerupTypes.BokChoy:
				title.text = "USAIN BOK CHOY";
				quote.text = "\"My name is Bok.. Lightning Bok.\"";
				image.sprite = bokChoySprite;
				desc.text = "Usain Bok Choy will give you his pace, making you faster!";
				break;

			case PowerupTypes.Plum:
				title.text = "PLUMARIO";
				quote.text = "\"PAHOOO.\"";
				image.sprite = plumSprite;
				desc.text = "Plumario will teach you to double jump!";
				break;

			case PowerupTypes.Chili:
				title.text = "ACHILIS";
				quote.text = "\"I would rather chill beside you than any army of thousands!\"";
				image.sprite = chiliSprite;
				desc.text = "Melee enemies will take damage when they hit you!";
				break;

			case PowerupTypes.Peppers:
				title.text = "KYLIAN MBAPPEPPER";
				quote.text = "\"If the pear needs you, you need to sacrifice.\"";
				image.sprite = pepperSprite;
				desc.text = "Mbappepper will have a 25% chance to deflect projectiles that hit you!!";
				break;

			case PowerupTypes.Grape:
				title.text = "ALEXANDER THE GRAPE";
				quote.text = "\"There is nothing impossible to pear that will try.\"";
				image.sprite = grapeSprite;
				desc.text = "Damage enemies when you run into them!";
				break;

			case PowerupTypes.Apple:
				title.text = "HIPPOCRATAPPLE";
				quote.text = "\"Let fruit be thy medicine and medicine by thy fruit.\"";
				image.sprite = appleSprite;
				desc.text = "Hippocratapple will heal you 10 hp every 10 seconds!";
				break;

			case PowerupTypes.Carrot:
				title.text = "Micarrot Jordan";
				quote.text = "\"Peart is what seperates the good from the great.\"";
				image.sprite = carrotSprite;
				desc.text = "Micarrot Jordan will lend you his shoes, allowing you to jump higher!";
				break;

			case PowerupTypes.Berry:
				title.text = "ALBERRY EINSTEIN";
				quote.text = "\"Pearitation is not responsible for fruits falling in love.\"";
				image.sprite = berrySprite;
				desc.text = "Alberry Einstein will make the calculation to make enemy melee attack damage enemies as well!";
				break;

			case PowerupTypes.Orange:
				title.text = "Oranheimer";
				quote.text = "\"Now I am become pearth, destroyer of worlds.\"";
				image.sprite = orangeSprite;
				desc.text = "Oraneimer will make the calculation to make enemy projectiles damage enemies as well!!";
				break;

			case PowerupTypes.Kiwi:
				title.text = "KIWIANU REEVES";
				quote.text = "\"My name can't be THAT tough to pearnounce.\"";
				image.sprite = kiwiSprite;
				desc.text = "Kiwianu Reeves will grant you his dodging skills, giving a 15% chance to dodge any attack!";
				break;

			case PowerupTypes.Broccoli:
				title.text = "MUHAMMAD BROCCALI";
				quote.text = "\"I don't count the pear-ups...\"";
				image.sprite = broccaliSprite;
				desc.text = "Muhammad Broccali will give you his gloves, increasing your attack range!";
				break;
		}
    }

    public void ApplyLeftBuff()
    {
        powerupScripts.ApplyPowerup(leftPowerupType);
        Continue();

	}

    public void ApplyRightBuff()
    {
		powerupScripts.ApplyPowerup(rightPowerupType);
        Continue();
	}

    public void Continue()
    {
		OnComplete?.Invoke();
		gameObject.SetActive(false);
	}
}
