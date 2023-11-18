using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseData
{
    // 현재 마우스 포인터가 있는 곳이 어떤 인벤토리 UI 인가를 알기 위한 변수
    public static InventoryUI interfaceMouseIsOver;
    // 드래그이미지를 임시적으로 가지고 있기 위한 변수
    public static GameObject tempItemBeingDragged;
    // 슬롯에 마우스가 들어왔냐 안들어왔냐를 판별하기 위한 변수
    public static GameObject slotHoveredOver;
}