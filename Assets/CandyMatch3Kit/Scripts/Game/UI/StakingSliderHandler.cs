using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class StakingSliderHandler : MonoBehaviour
{
    public Slider stakingSlider;
    public TextMeshProUGUI stakingAmountText;
    
    public delegate void StakingAmountChangedHandler(int newAmount);
    public event StakingAmountChangedHandler OnStakingAmountChanged;

    // List of staking amounts corresponding to slider positions
    private List<int> stakingAmounts = new List<int> { 0, 1, 10, 100 };

    private int selectedStakingAmount;
    
    

    void Start()
    {
        // Ensure the slider uses whole numbers
        stakingSlider.wholeNumbers = true;
        stakingSlider.minValue = 0;
        stakingSlider.maxValue = stakingAmounts.Count - 1;

        // Add listener for value changes
        stakingSlider.onValueChanged.AddListener(OnSliderValueChanged);

        // Initialize display
        OnSliderValueChanged(stakingSlider.value);
    }

    void OnSliderValueChanged(float value)
    {
        int index = (int)value;
        selectedStakingAmount = stakingAmounts[index];
        UpdateStakingAmountText();
        OnStakingAmountChanged?.Invoke(selectedStakingAmount);
    }

    void UpdateStakingAmountText()
    {
        // Update the UI text to show the selected staking amount
        stakingAmountText.text = $"Amount: {selectedStakingAmount} sui";
    }

    public int GetSelectedStakingAmount()
    {
        return selectedStakingAmount;
    }
}