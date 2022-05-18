using UnityEngine;
using UI;
using Entities;
using UnityEngine.EventSystems;

namespace Board
{
    public class ShipPlacementLogic : MonoBehaviour
    {
        public GameBoard board;

        private Camera mainCam;
        private Ship currentShip;
        private GameObject currentShipObject;
        private Transform currentShipTransform;
        private float raycastOffset = 5;

        private void Awake()
        {
            mainCam = Camera.main;
            ShipButtonManager.SendSelectedShip += SetCurrentShip;
        }

        private void Update()
        {
            //TODO: move Input into its own script
            if (currentShip == null)
                return;

            CalculateShipGridPos(Input.mousePosition);
                
            if (Input.GetMouseButtonDown(0))
                PlaceCurrentShip();
            if (Input.GetKeyDown(KeyCode.R))
                RotateShipVisual();
        }

        private void SetCurrentShip(Ship _selectedShip)
        {
            if (_selectedShip == null)
            {
                Debug.LogError("Selected ship is null!");
                return;
            }

            if (currentShip != null)
                DestroyCurrentShip();

            currentShip = _selectedShip;
            currentShipObject = Instantiate(currentShip.visual);
            currentShipTransform = currentShipObject.transform;
        }

        private void DestroyCurrentShip()
        {
            Destroy(currentShipObject);
            ResetCurrentShip();
        }

        private void ResetCurrentShip()
        {
            currentShip = null;
            currentShipObject = null;
            currentShipTransform = null;
        }

        private void CalculateShipGridPos(Vector3 _pos)
        {
            var shipStartTile = RaycastFromCamera(_pos);
            if (shipStartTile == null)
                return;

            var endPos = CalculateEndTilePos(shipStartTile.position, currentShipTransform.rotation);
            endPos.y = raycastOffset;
            var shipEndTile = RaycastFromPosition(endPos);

            if (!board.IsValidTile(shipStartTile) || !board.IsValidTile(shipEndTile))
                return;

            currentShipTransform.position = shipStartTile.position;
        }

        private Vector3 CalculateEndTilePos(Vector3 _startPos, Quaternion _rotation)
        {
            return _startPos + (_rotation * new Vector3(0, 0, currentShip.hp - 1));
        }


        private Transform RaycastFromCamera(Vector3 _pos)
        {
            Ray ray = mainCam.ScreenPointToRay(_pos);
            return ShootRaycast(ray);
        }

        private Transform RaycastFromPosition(Vector3 _pos)
        {
            Ray ray = new Ray(_pos, Vector3.down);
            return ShootRaycast(ray);
        }

        private Transform ShootRaycast(Ray _ray)
        {
            if (!Physics.Raycast(_ray, out RaycastHit hit, 100, LayerMask.GetMask("Tiles")))
                return null;

            return hit.transform;
        }

        private void PlaceCurrentShip()
        {
            ResetCurrentShip();
            ShipButtonManager.ShipPlaced?.Invoke();
        }

        private void RotateShipVisual()
        {
            var endRot = new Vector3(currentShipTransform.eulerAngles.x, currentShipTransform.eulerAngles.y + 90, currentShipTransform.eulerAngles.z);
            var endPos = CalculateEndTilePos(currentShipTransform.position, Quaternion.Euler(endRot));
            endPos.y = raycastOffset;
            var shipEndTile = RaycastFromPosition(endPos);

            if (board.IsValidTile(shipEndTile))
                currentShipTransform.Rotate(Vector3.up, 90);
        }
    }
}