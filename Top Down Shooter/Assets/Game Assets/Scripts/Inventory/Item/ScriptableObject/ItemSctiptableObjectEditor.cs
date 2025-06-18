using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ItemSctiptableObject))]
public class ItemSctiptableObjectEditor : Editor
{
    ItemSctiptableObject _itemSctiptableObject;
    SerializedObject _serializedItemSctiptableObject;
    SerializedProperty _TypeProp;
    SerializedProperty _TypeValueProp;
    SerializedProperty _IconProp;
    private void OnEnable()
    {
        _itemSctiptableObject = target as ItemSctiptableObject;
        _serializedItemSctiptableObject = new SerializedObject(_itemSctiptableObject);
        _TypeProp = _serializedItemSctiptableObject.FindProperty("Type");
        _TypeValueProp = _serializedItemSctiptableObject.FindProperty("ValueType");
        _IconProp = _serializedItemSctiptableObject.FindProperty("Icon");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI(); //будет отрисовывать оригинальный инспектор
        _serializedItemSctiptableObject.Update();

        DrawCustomInspector();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(_itemSctiptableObject);
            _serializedItemSctiptableObject.ApplyModifiedProperties();
        }
    }

    private void DrawCustomInspector()
    {
        EditorGUILayout.PropertyField(_TypeProp, new GUIContent("Тип предмета"));

        if(_itemSctiptableObject.Type == ItemType.None)
            EditorGUILayout.HelpBox("При выборе типа придмета будут доступны дополнительные поля для заполнения!", MessageType.Info);

        var old = EditorStyles.label.normal.textColor;
        var oldActive = EditorStyles.label.focused.textColor;

        var color = Color.white;

        switch (_itemSctiptableObject.ValueType)
        {
            case ItemValueType.Rare:
                color = Color.magenta;
                break;
            case ItemValueType.ExtraRare:
                color = Color.yellow;
                break;
            case ItemValueType.Insane:
                color = Color.red;
                break;
        }

        EditorStyles.label.normal.textColor = color;
        EditorStyles.label.focused.textColor = color;

        EditorGUILayout.PropertyField(_TypeValueProp, new GUIContent("Тип стоимости", "Отоброжение важности и редкости"));

        EditorStyles.label.normal.textColor = old;
        EditorStyles.label.focused.textColor = oldActive;

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.PropertyField(_IconProp, new GUIContent("Иконка"));

        _itemSctiptableObject.Cost = EditorGUILayout.IntField(
            new GUIContent("Стоимость", "Стоимость предмета"),
            _itemSctiptableObject.Cost
        );

        switch (_itemSctiptableObject.Type)
        {
            case ItemType.Medical:
                _itemSctiptableObject.Heal = EditorGUILayout.FloatField(
                    new GUIContent("Восстоновление здоровья", "Сколько восстановить персонаж при использовании"),
                    _itemSctiptableObject.Heal
                );
                break;

            case ItemType.ArmourHead:
            case ItemType.ArmourChest:
            case ItemType.ArmourLegs:
                _itemSctiptableObject.SlowDownMovementPercent = EditorGUILayout.FloatField(
                    new GUIContent("Замедление", "Процент замедления передвижения персонажа при использовании данной экипировки"),
                    _itemSctiptableObject.SlowDownMovementPercent
                );
                _itemSctiptableObject.DamageBlockPercent = EditorGUILayout.FloatField(
                    new GUIContent("Блок урона", "Процент блокирования урона от попадания"),
                    _itemSctiptableObject.DamageBlockPercent
                );
                break;

            case ItemType.Backpack:
                _itemSctiptableObject.SlotCount = EditorGUILayout.IntField(
                    new GUIContent("Кол-во слотов", "Количество слотов получаемое при ношении рюкзака"),
                    _itemSctiptableObject.SlotCount
                );
                break;
                
            case ItemType.WeaponMain: case ItemType.WeaponSecondary:
                _itemSctiptableObject.DamageDeal = EditorGUILayout.IntField(
                    new GUIContent("Урон"),
                    _itemSctiptableObject.DamageDeal
                );
                _itemSctiptableObject.ArmourPenetration = EditorGUILayout.FloatField(
                    new GUIContent("Пробитие брони", "Шанс или процент игнорирования броникомплекта противника"),
                    _itemSctiptableObject.ArmourPenetration
                );
                _itemSctiptableObject.MaxBulletCount = EditorGUILayout.IntField(
                    new GUIContent("Кол-во патрон в магазине"),
                    _itemSctiptableObject.MaxBulletCount
                );
                break;
        }
    }
}
#endif