using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class CustomBlockBar : MonoBehaviour
    {
        [Header("Links")]
        [SerializeField] private RectTransform blockPrefab;
        [SerializeField] private RectTransform blocksParent;
        [SerializeField] private ParticleSystem reduceParticles;
        [SerializeField] private Animator animator;
        
        [Header("Settings")]
        [SerializeField] private float particlesOffset;
        [SerializeField] private bool needDamageAnimation;
        

        [Header("Monitoring")]
        [SerializeField] private int currentCount;
        [SerializeField] private int maxCount;
        [SerializeField] private List<RectTransform> blocks;


        public void SetUp(int blockMaxCount, int blocksCurCount)
        {
            maxCount = blockMaxCount;
            currentCount = blocksCurCount;
            if (blocks != null)
                foreach (var rectTransform in blocks)
                    Destroy(rectTransform.gameObject);

            blocks = new List<RectTransform>();

            for (int i = 0; i < currentCount; i++) blocks.Add(SpawnBlock());
        }

        public void AddBlock()
        {
            blocks.Add(SpawnBlock());
            currentCount++;
        }

        public void RemoveBlock()
        {
            currentCount--;
            if (currentCount < 0)
            {
                Debug.Log("Bro already 0 blocks");
                return;
            }

            var block = blocks[currentCount];
            if (reduceParticles)
            {
                var newParticles = Instantiate(reduceParticles);
                newParticles.transform.position = block.transform.position;

                // var newEndParticles = Instantiate(endParticles, targetCamera.transform, true);

                var main = Camera.main;
                var screenToWorldPoint =
                    main.ScreenToWorldPoint(block.transform.position + main.transform.forward * particlesOffset);
                newParticles.transform.position = screenToWorldPoint;
            }

            if(needDamageAnimation) animator.SetTrigger("Damage");
            
            Destroy(block.gameObject);
        }


        private RectTransform SpawnBlock()
        {
            var newBlock = Instantiate(blockPrefab, blocksParent);
            return newBlock;
        }
    }
}