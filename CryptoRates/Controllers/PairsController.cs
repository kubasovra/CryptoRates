using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CryptoRates.Data;
using Microsoft.AspNetCore.Identity;
using CryptoRates.Models;
using CryptoRates.Data.DTO;

namespace CryptoRates.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PairsController : ControllerBase
    {
        private readonly CryptoContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PairsController(CryptoContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Pairs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PairDTO>>> GetPairs()
        {
            //Yet it only shows all pairs, not for particular user. Gonna fix it when it will be possible to add new pairs
            List<PairDTO> pairDTOs = await _context.Pairs.Include(p => p.User).Include(p => p.FirstCurrency).Include(p => p.SecondCurrency).Select(p => PairToDTO(p)).ToListAsync();
            return pairDTOs;
        }

        // GET: Pairs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PairDTO>> GetPair(int id)
        {
            var pairDTO = PairToDTO(await _context.Pairs.FindAsync(id));

            if (pairDTO == null)
            {
                return NotFound();
            }

            return pairDTO;
        }

        // PUT: Pairs/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPair(int id, Pair pair)
        {
            if (id != pair.PairId)
            {
                return BadRequest();
            }

            _context.Entry(pair).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PairExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: Pairs
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<PairDTO>> PostPair(PairDTO pairDTO)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(this.User);
            _context.Pairs.Add(PairFromDTO(pairDTO, currentUser, _context));
            await _context.SaveChangesAsync();

            return NoContent();
            //return CreatedAtAction(nameof(GetPair), new { id = pairDTO.PairId }, pairDTO);
        }

        // DELETE: Pairs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PairDTO>> DeletePair(int id)
        {
            var pair = await _context.Pairs.FindAsync(id);
            if (pair == null)
            {
                return NotFound();
            }

            _context.Pairs.Remove(pair);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PairExists(int id)
        {
            return _context.Pairs.Any(e => e.PairId == id);
        }

        private static PairDTO PairToDTO(Pair pair)
        {
            return new PairDTO(pair.PairId, pair.User.Id)
            {
                FirstCurrencyName = pair.FirstCurrency.Name,
                FirstCurrencySymbol = pair.FirstCurrency.Symbol,
                FirstCurrencyImageUrl = pair.FirstCurrency.ImageURL,
                FirstCurrencyPageUrl = pair.FirstCurrency.WebPage,
                SecondCurrencyName = pair.SecondCurrency.Name,
                SecondCurrencySymbol = pair.SecondCurrency.Symbol,
                SecondCurrencyImageUrl = pair.SecondCurrency.ImageURL,
                SecondCurrencyPageUrl = pair.SecondCurrency.WebPage,
                PriceFirstToSecond = pair.PriceFirstToSecond,
                PreviousPriceFirstToSecond = pair.PreviousPriceFirstToSecond,
                TargetPrice = pair.TargetPrice,
                TargetPriceAbsoluteChange = pair.TargetPriceAbsoluteChange,
                TargetPricePercentChange = pair.TargetPricePercentChange,
                IsNotifyOnPrice = pair.IsNotifyOnPrice,
                IsNotifyOnAbsoluteChange = pair.IsNotifyOnAbsoluteChange,
                IsNotifyOnPercentChange = pair.IsNotifyOnPercentChange
            };
        }

        private static Pair PairFromDTO(PairDTO pairDTO, ApplicationUser currentUser, CryptoContext context )
        {
            return new Pair()
            {
                User = currentUser,
                FirstCurrency = context.Currencies.FirstOrDefault(c => c.Name == pairDTO.FirstCurrencyName),
                SecondCurrency = context.Currencies.FirstOrDefault(c => c.Name == pairDTO.SecondCurrencyName),
                PriceFirstToSecond = pairDTO.PriceFirstToSecond,
                PreviousPriceFirstToSecond = pairDTO.PreviousPriceFirstToSecond,
                TargetPrice = pairDTO.TargetPrice,
                TargetPriceAbsoluteChange = pairDTO.TargetPriceAbsoluteChange,
                TargetPricePercentChange = pairDTO.TargetPricePercentChange,
                IsNotifyOnPrice = pairDTO.IsNotifyOnPrice,
                IsNotifyOnAbsoluteChange = pairDTO.IsNotifyOnAbsoluteChange,
                IsNotifyOnPercentChange = pairDTO.IsNotifyOnPercentChange
            };
        }
    }
}
